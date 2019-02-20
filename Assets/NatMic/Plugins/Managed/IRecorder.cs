/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core.Recorders {

    using System;

    public interface IRecorder : IDisposable {
        /// <summary>
        /// Audio format of audio data that will be recorded
        /// </summary>
        Format Format { get; }
        /// <summary>
        /// Start recording
        /// </summary>
        void StartRecording ();
        /// <summary>
        /// Commit audio data to be recorded
        /// </summary>
        /// <param name="samples">Audio sample buffer</param>
        /// <param name="timestamp">Timestamp for the sample buffer in nanoseconds</param>
        void CommitSamples (float[] samples, long timestamp);
    }
}