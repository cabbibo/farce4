/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core.Platforms {

    using System;
    using System.Runtime.InteropServices;

    public static class NatMicBridge {
        
        private const string Assembly =
        #if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
        "__Internal";
        #else
        "NatMic";
        #endif

        public delegate void AudioDataCallback (IntPtr sampleBuffer, int sampleCount, long timestamp);

        #if UNITY_IOS || UNITY_WEBGL || UNITY_STANDALONE || UNITY_EDITOR
        [DllImport(Assembly, EntryPoint = "NMDeviceCount")]
        public static extern int DeviceCount ();
        [DllImport(Assembly, EntryPoint = "NMAcquireDevice")]
        public static extern IntPtr AcquireDevice (int index);
        [DllImport(Assembly, EntryPoint = "NMIsRecording")]
        public static extern bool IsRecording ();
        [DllImport(Assembly, EntryPoint = "NMStartRecording")]
        public static extern void StartRecording (AudioDataCallback dataCallback, IntPtr device, int sampleRate, int channelCount);
        [DllImport(Assembly, EntryPoint = "NMStopRecording")]
        public static extern void StopRecording ();
        [DllImport(Assembly, EntryPoint = "NMCurrentTimestamp")]
        public static extern long CurrentTimestamp ();
        #else
        public static int DeviceCount () { return 0; }
        public static IntPtr AcquireDevice (int index) { return IntPtr.Zero; }
        public static bool IsRecording () { return false; }
        public static void StartRecording (AudioDataCallback dataCallback, IntPtr device, int sampleRate, int channelCount) {}
        public static void StopRecording () {}
        public static long CurrentTimestamp () { return 0L; }
        #endif
    }
}