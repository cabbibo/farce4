/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core {

    using System;
    using System.Linq;
    using Docs;

    /// <summary>
    /// Audio capture device
    /// </summary>
    [Doc(@"Device")]
    public abstract class Device : IEquatable<Device> { // We can't rely on reference equality

        #region --Statics--
        /// <summary>
        /// Default audio capture device
        /// </summary>
        [Doc(@"Default")]
        public static Device Default { get { return NatMic.Devices[0]; }}
        #endregion


        #region ---Properties---
        /// <summary>
        /// Display-friendly device name
        /// </summary>
        [Doc(@"Name")]
        public abstract string Name { get; }
        /// <summary>
        /// Device unique ID
        /// </summary>
        [Doc(@"UID")]
        public abstract string UID { get; }
        /// <summary>
        /// Get or set Adaptive Echo Cancellation (AEC) on the device
        /// </summary>
        [Doc(@"EchoCancellation")]
        public abstract bool EchoCancellation { get; set; }
        #endregion


        #region --Typecasting--

        public override string ToString () {
            return Name + " (" + UID + ")";
        }

        public bool Equals (Device other) {
            return this.Equals((object)other);
        }

        public override bool Equals (object obj) {
            return obj != null && obj is Device && (obj as Device).UID == UID;
        }

        public override int GetHashCode () {
            return UID.GetHashCode();
        }

        public static bool operator == (Device lhs, Device rhs) {
            if ((object)lhs == null && (object)rhs == null) return true;
            return (object)lhs != null && lhs.Equals(rhs);
        }

        public static bool operator != (Device lhs, Device rhs) {
            return !(lhs == rhs);
        }

        public static implicit operator Device (string name) {
            return (
                from device in NatMic.Devices
                where device.Name == name
                select device
            ).FirstOrDefault();
        }
        #endregion
    }
}