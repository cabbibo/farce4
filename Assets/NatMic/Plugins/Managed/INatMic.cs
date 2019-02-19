/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core.Platforms {

    using UnityEngine;

    public interface INatMic {

        #region --Properties--
        Device[] Devices { get; }
        bool IsRecording { get; }
        long CurrentTimestamp { get; }
        string RecordingPath { get; } // Used by Recorders
        #endregion

        #region --Operations--
        void StartRecording (Device device, Format format, SampleBufferCallback dataCallback);
        void StopRecording ();
        #endregion
    }
}