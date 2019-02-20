/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core.Recorders {

    using UnityEngine;
    using Utilities;
    using Docs;

    /// <summary>
    /// A recorder that doubles as a realtime AudioClip
    /// </summary>
    [Doc(@"RealtimeClip")]
    public sealed class RealtimeClip : IRecorder {
        
        #region --Op vars--
        /// <summary>
        /// Format of audio data that will be recorded
        /// </summary>
        [Doc(@"IRecorderFormat")]
        public Format Format { get; private set; }
        private AudioClip clip;
        private CircularBuffer buffer;
        private readonly object fence = new object();
        #endregion


        #region --Client API--

        /// <summary>
        /// Create a realtime clip
        /// </summary>
        /// <param name="format">Format of the clip</param>
        [Doc(@"RealtimeClipCtor")]
        public RealtimeClip (Format format) {
            Format = format;
            buffer = new CircularBuffer();
            clip = AudioClip.Create(
                "NatMic Realtime Clip",
                int.MaxValue,
                Format.channelCount,
                Format.sampleRate,
                true,
                OnAudioDataRead
            );
        }

        /// <summary>
        /// Start recording. This does nothing.
        /// </summary>
        [Doc(@"IRecorderStartRecording", @"IRecorderStartRecordingDescription")]
        public void StartRecording () {}

        /// <summary>
        /// Commit audio data to be recorded
        /// </summary>
        /// <param name="samples">Audio sample buffer</param>
        /// <param name="timestamp">Timestamp for the sample buffer in nanoseconds</param>
        [Doc(@"IRecorderCommitSamples")]
        public void CommitSamples (float[] samples, long timestamp) {
            lock (fence) buffer.Write(samples);
        }

        /// <summary>
        /// Stop writing and destroy the audio clip
        /// </summary>
        [Doc(@"IRecorderDispose")]
        public void Dispose () {
            AudioClip.Destroy(clip);
            clip = null;
        }
        #endregion


        #region --Operations--

        private void OnAudioDataRead (float[] destination) {
            lock (fence)
                if (buffer.CanRead(destination.Length))
                    buffer.Read(destination, destination.Length);
        }

        public static implicit operator AudioClip (RealtimeClip clip) {
            return clip.clip;
        }

        public static implicit operator bool (RealtimeClip clip) {
            return clip != null;
        }
        #endregion
    }
}