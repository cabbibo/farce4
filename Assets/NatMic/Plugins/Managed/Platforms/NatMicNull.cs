/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core.Platforms {

    using UnityEngine;

    public sealed class NatMicNull : INatMic {

        public Device[] Devices { get { return null; }}
        
        public bool IsRecording { get { return false; }}

        public long CurrentTimestamp { get { return 0L; }}

        public string RecordingPath { get { return ""; }}

        public NatMicNull () {
            Debug.LogWarning("NatMic: NatMic 1.2 is not supported on this platform");
        }

        public void StartRecording (Device device, Format format, SampleBufferCallback dataCallback) {}

        public void StopRecording () {}
    }
}