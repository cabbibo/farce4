/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core.Platforms {

    using UnityEngine;

    public sealed class DeviceAndroid : Device {

        #region --Ctor--

        private readonly AndroidJavaObject nativeDevice;

        public DeviceAndroid (AndroidJavaObject nativeDevice) {
            this.nativeDevice = nativeDevice;
        }

        ~DeviceAndroid () {
            nativeDevice.Dispose();
        }
        #endregion

        
        #region ---Properties---
        
        public override string Name {
            get { return nativeDevice.Get<string>("name"); }
        }

        public override string UID {
            get { return nativeDevice.Get<string>("uid"); }
        }

        public override bool EchoCancellation {
            get { return nativeDevice.Get<bool>("echoCancellation"); }
            set { nativeDevice.Set("echoCancellation", value); }
        }
        #endregion


        #region --Typecasting--

        public static explicit operator AndroidJavaObject (DeviceAndroid device) {
            return device.nativeDevice;
        }
        #endregion
    }
}