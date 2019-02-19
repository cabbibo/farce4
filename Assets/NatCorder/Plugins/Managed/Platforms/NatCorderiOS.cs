/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using NatRenderU;
    using IRecorder = System.IntPtr;

    public class NatCorderiOS : INatCorder {

        #region --Op vars--
        protected string recordingDirectory;
        protected TextureFormat readbackFormat;
        protected VideoFormat videoFormat;
        protected RecordingCallback recordingCallback;
        private Dispatcher dispatcher;
        protected ReadbackContext readbackContext;
        protected IRecorder recorder;
        protected SynchronizationHint SyncHint = SynchronizationHint.LessFrameTime;
        private static NatCorderiOS instance { get { return NatCorder.Implementation as NatCorderiOS; }}
        #endregion
        

        #region --Properties--
        public bool IsRecording { get { return recorder != IntPtr.Zero; }}
        public long CurrentTimestamp { get { return NatCorderBridge.CurrentTimestamp(); }}
        #endregion


        #region --Operations--

        public NatCorderiOS () {
            recordingDirectory = Application.persistentDataPath;
            readbackFormat = TextureFormat.BGRA32;
            SyncHint = SynchronizationHint.LessMemory; // Greatly improves memory stability on iOS
            Debug.Log("NatCorder: Initialized NatCorder 1.4 iOS backend");
        }

        public virtual void StartRecording (Container container, VideoFormat videoFormat, AudioFormat audioFormat, RecordingCallback videoCallback) {
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
            this.readbackContext = ReadbackContext.Create(readbackFormat, SyncHint);
            // Start recording
            recorder = NatCorderBridge.StartRecording(
                recordingDirectory,
                OnVideo,
                container,
                videoFormat.width,
                videoFormat.height,
                videoFormat.framerate,
                videoFormat.bitrate,
                videoFormat.keyframeInterval,
                audioFormat.sampleRate,
                audioFormat.channelCount
            );
        }

        public virtual void StopRecording () {
            // We need to queue, but we want IsRecording to be false immediately
            var localRecorder = this.recorder;
            this.recorder = IntPtr.Zero;
            readbackContext.Readback(null, ptr => localRecorder.StopRecording());
            readbackContext.Dispose();
            readbackContext = null;
        }

        public virtual RenderTexture AcquireFrame () {
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

        public virtual void CommitFrame (RenderTexture frame, long timestamp) {
            var localRecorder = this.recorder;
            readbackContext.Readback(frame, framePtr => {
                localRecorder.EncodeFrame(framePtr, timestamp);
                dispatcher.Dispatch(() => frame.Release());
            });
        }

        public virtual void CommitSamples (float[] sampleBuffer, long timestamp) {
            recorder.EncodeSamples(sampleBuffer, sampleBuffer.Length, timestamp);
        }
        #endregion


        #region --Callbacks--

        [MonoPInvokeCallback(typeof(RecordingCallback))]
        private static void OnVideo (string path) {
            instance.dispatcher.Dispatch(() => instance.recordingCallback(path));
            instance.dispatcher.Dispose();
            instance.dispatcher = null;
        }
        #endregion
    }
}