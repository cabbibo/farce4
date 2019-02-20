/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core.Platforms {

    using System;
    using System.Runtime.InteropServices;

    public static class DeviceBridge {
        
        private const string Assembly =
        #if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
        "__Internal";
        #else
        "NatMic";
        #endif

        #if UNITY_IOS || UNITY_WEBGL || UNITY_STANDALONE || UNITY_EDITOR
        [DllImport(Assembly, EntryPoint = "NMFreeDevice")]
        public static extern void FreeDevice (this IntPtr device);
        [DllImport(Assembly, EntryPoint = "NMDeviceName")]
        public static extern IntPtr DeviceName (this IntPtr device);
        [DllImport(Assembly, EntryPoint = "NMDeviceUID")]
        public static extern IntPtr DeviceUID (this IntPtr device);
        [DllImport(Assembly, EntryPoint = "NMDeviceGetEchoCancellation")]
        public static extern bool DeviceGetEchoCancellation (this IntPtr device);
        [DllImport(Assembly, EntryPoint = "NMDeviceSetEchoCancellation")]
        public static extern bool DeviceSetEchoCancellation (this IntPtr device, bool aec);
        
        #else
        public static void FreeDevice (this IntPtr device) {}
        public static IntPtr DeviceName (this IntPtr device) { return IntPtr.Zero; }
        public static IntPtr DeviceUID (this IntPtr device) { return IntPtr.Zero; }
        public static bool DeviceGetEchoCancellation (this IntPtr device) { return false; }
        public static void DeviceSetEchoCancellation (this IntPtr device, bool aec) {}
        #endif
    }
}