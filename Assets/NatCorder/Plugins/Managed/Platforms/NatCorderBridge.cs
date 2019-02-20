/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using System;
    using System.Runtime.InteropServices;

    public static class NatCorderBridge {

        private const string Assembly =
        #if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
        "__Internal";
        #else
        "NatCorder";
        #endif

        #if UNITY_IOS || UNITY_WEBGL || UNITY_STANDALONE || UNITY_EDITOR
        [DllImport(Assembly, EntryPoint = "NCStartRecording")]
        public static extern IntPtr StartRecording (
            #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            [MarshalAs(UnmanagedType.LPWStr)]
            #endif
            string recordingDirectory,
            RecordingCallback recordingCallback,
            Container container,
            int width,
            int height,
            int framerate,
            int bitrate,
            int keyframes,
            int sampleRate,
            int channelCount
        );
        [DllImport(Assembly, EntryPoint = "NCStopRecording")]
        public static extern void StopRecording (this IntPtr recorder);
        [DllImport(Assembly, EntryPoint = "NCEncodeFrame")]
        public static extern void EncodeFrame (this IntPtr recorder, IntPtr textureID, long timestamp);
        [DllImport(Assembly, EntryPoint = "NCEncodeSamples")]
        public static extern void EncodeSamples (this IntPtr recorder, float[] sampleBuffer, int sampleCount, long timestamp);
        [DllImport(Assembly, EntryPoint = "NCCurrentTimestamp")]
        public static extern long CurrentTimestamp ();

        #else
        public static IntPtr StartRecording (
            string recordingDirectory,
            RecordingCallback recordingCallback,
            Container container,
            int width,
            int height,
            int framerate,
            int bitrate,
            int keyframes,
            int sampleRate,
            int channelCount
        ) { return IntPtr.Zero; }
        public static void StopRecording (this IntPtr recorder) {}
        public static void EncodeFrame (this IntPtr recorder, IntPtr textureID, long timestamp) {}
        public static void EncodeSamples (this IntPtr recorder, float[] sampleBuffer, int sampleCount, long timestamp) {}
        public static long CurrentTimestamp () { return 0L; }
        #endif
    }
}