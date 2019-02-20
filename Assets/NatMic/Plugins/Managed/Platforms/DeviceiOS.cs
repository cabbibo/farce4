/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core.Platforms {

    using System;
    using System.Runtime.InteropServices;

    public class DeviceiOS : Device {
        
        #region --Ctor--

        protected readonly IntPtr handle;

        public DeviceiOS (IntPtr handle) {
            this.handle = handle;
        }

        ~DeviceiOS () {
            handle.FreeDevice();
        }
        #endregion


        #region ---Properties---
        public override string Name {
            get {
                var nativeStr = handle.DeviceName();
                var uid = Marshal.PtrToStringAuto(nativeStr);
                Marshal.FreeCoTaskMem(nativeStr);
                return uid;
            }
        }

        public override string UID {
            get {
                var nativeStr = handle.DeviceUID();
                var uid = Marshal.PtrToStringAuto(nativeStr);
                Marshal.FreeCoTaskMem(nativeStr);
                return uid;
            }
        }
        
        public override bool EchoCancellation {
            get { return handle.DeviceGetEchoCancellation(); }
            set { handle.DeviceSetEchoCancellation(value); }
        }
        #endregion


        #region --Typecasting--

        public static explicit operator IntPtr (DeviceiOS device) {
            return device.handle;
        }
        #endregion
    }
}