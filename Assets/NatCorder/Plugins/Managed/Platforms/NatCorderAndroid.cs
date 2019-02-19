/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using UnityEngine;
    using UnityEngine.Scripting;
    using System;
    using System.Runtime.InteropServices;
    using NatRenderU;
    using NatRenderU.Dispatch;

    public sealed class NatCorderAndroid : AndroidJavaProxy, INatCorder {

        #region --Op vars--
        private string recordingDirectory;
        private VideoFormat videoFormat;
        private RecordingCallback recordingCallback;
        private Dispatcher dispatcher;
        private ReadbackContext readbackContext;
        private AndroidJavaObject recorder;
        private readonly AndroidJavaObject renderDispatch;
        private const SynchronizationHint SyncHint = SynchronizationHint.LessFrameTime;
        #endregion


        #region --Properties--
        public bool IsRecording { get { return recorder != null; } }

        public long CurrentTimestamp {
            get {
                AndroidJNI.AttachCurrentThread();
                using (var bridge = new AndroidJavaClass("com.yusufolokoba.natcorder.Bridge"))
                    return bridge.CallStatic<long>("currentTimestamp");
            }
        }
        #endregion


        #region --Operations--

        public NatCorderAndroid () : base("com.yusufolokoba.natcorder.NatCorderDelegate") {
            recordingDirectory = Application.persistentDataPath;
            renderDispatch = new AndroidJavaObject("com.yusufolokoba.natrender.RenderDispatch");
            var dispatch = Dispatcher.Create(DispatchThread.RenderThread);
            DispatchUtility.onFrame += () => dispatch.Dispatch(() => renderDispatch.Call("invoke"));
            Debug.Log("NatCorder: Initialized NatCorder 1.4 Android backend");
        }

        public void StartRecording (Container container, VideoFormat videoFormat, AudioFormat audioFormat, RecordingCallback videoCallback) {
            // Make sure that recording size is even
            videoFormat = new VideoFormat(
                videoFormat.width >> 1 << 1,
                videoFormat.height >> 1 << 1,
                videoFormat.framerate,
                videoFormat.bitrate,
                videoFormat.keyframeInterval
            );
            // Save state
            this.videoFormat = videoFormat;
            this.recordingCallback = videoCallback;
            this.dispatcher = Dispatcher.Create(DispatchThread.MainThread);
            this.readbackContext = ReadbackContext.Create(0, SyncHint);
            // Start recording
            using (var bridge = new AndroidJavaClass("com.yusufolokoba.natcorder.Bridge"))
                recorder = bridge.CallStatic<AndroidJavaObject>(
                    "startRecording",
                    recordingDirectory,
                    renderDispatch,
                    this,
                    (int)container,
                    videoFormat.width,
                    videoFormat.height,
                    videoFormat.framerate,
                    videoFormat.bitrate,
                    videoFormat.keyframeInterval,
                    audioFormat.sampleRate,
                    audioFormat.channelCount
                );
        }

        public void StopRecording () {
            // We need to queue, but we want IsRecording to be false immediately
            var localRecorder = this.recorder;
            this.recorder = null;
            readbackContext.Readback(null, ptr => localRecorder.Call("stopRecording"));
            readbackContext.Dispose();
            readbackContext = null;
        }

        public RenderTexture AcquireFrame () {
            var frameTexture = new RenderTexture(
                videoFormat.width,
                videoFormat.height,
                24,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.sRGB
            );
            frameTexture.antiAliasing = 1;
            frameTexture.useMipMap = false;
            frameTexture.Create();
            var _ = frameTexture.colorBuffer;
            return frameTexture;
        }

        public void CommitFrame (RenderTexture frame, long timestamp) {
            var localRecorder = this.recorder;
            var textureHandle = (IntPtr)GCHandle.Alloc(frame, GCHandleType.Normal);
            readbackContext.Readback(frame, framePtr => {
                localRecorder.Call("encodeFrame", framePtr.ToInt64(), textureHandle.ToInt64(), timestamp);
            });
        }

        public void CommitSamples (float[] sampleBuffer, long timestamp) {
            AndroidJNI.AttachCurrentThread();
            recorder.Call("encodeSamples", sampleBuffer, timestamp);
        }
        #endregion


        #region --Callbacks--

        [Preserve]
        private void onEncode (long textureHandle) {
            // This will never happen by default
            // But keep it in case devs bypass RenderTexture to commit external frame
            if (textureHandle == 0L)
                return;
            // Release RenderTexture
            var frameHandle = (GCHandle)(IntPtr)textureHandle;
            var frame = frameHandle.Target as RenderTexture;
            frameHandle.Free();
            dispatcher.Dispatch(() => frame.Release());
        }

        [Preserve]
        private void onVideo (string path) {
            dispatcher.Dispatch(() => recordingCallback(path));
            dispatcher.Dispose();
            dispatcher = null;
        }
        #endregion
    }
}