/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core.Recorders {

    using UnityEngine;
    using System;
    using System.IO;
    using Utilities;
    using Docs;

    /// <summary>
    /// Recorder for recording audio to an AudioClip
    /// </summary>
    [Doc(@"ClipRecorder")]
    public sealed class ClipRecorder : IRecorder {

        #region --Op vars--
        /// <summary>
        /// Format of audio data that will be recorded
        /// </summary>
        [Doc(@"IRecorderFormat")]
        public Format Format { get; private set; }
        private readonly Action<AudioClip> Callback;
        private MemoryStream audioBuffer;
        #endregion


        #region --Client API--

        /// <summary>
        /// Create an AudioClip recorder
        /// </summary>
        /// <param name="format">Audio data format</param>
        [Doc(@"ClipRecorderCtor")]
        public ClipRecorder (Format format, Action<AudioClip> callback) {
            Format = format;
            Callback = callback;
        }

        /// <summary>
        /// Start recording.
        /// </summary>
        [Doc(@"IRecorderStartRecording", @"IRecorderStartRecordingDescription")]
        public void StartRecording () {
            audioBuffer = new MemoryStream();
        }

        /// <summary>
        /// Commit audio data to be recorded
        /// </summary>
        /// <param name="samples">Audio sample buffer</param>
        /// <param name="timestamp">Timestamp for the sample buffer in nanoseconds</param>
        [Doc(@"IRecorderCommitSamples")]
        public void CommitSamples (float[] samples, long timestamp) {
            // State checking
            if (audioBuffer == null) {
                Debug.LogError("NatMic Error: ClipRecorder::StartRecording must be called before committing samples");
                return;
            }
            if (samples == null) {
                Debug.LogError("NatMic Error: ClipRecorder cannot commit samples with null buffer");
                return;
            }
            // Write
            var byteSamples = new byte[Buffer.ByteLength(samples)];
            Buffer.BlockCopy(samples, 0, byteSamples, 0, byteSamples.Length);
            audioBuffer.Write(byteSamples, 0, byteSamples.Length);
        }

        /// <summary>
        /// Stop writing and invoke recording callback
        /// </summary>
        [Doc(@"IRecorderDispose")]
        public void Dispose () {
            if (audioBuffer == null)
                return;
            var byteSamples = audioBuffer.ToArray();
            var totalSampleCount = byteSamples.Length / sizeof(float); 
            var samples = new float[totalSampleCount];  
            var recordingName = string.Format("recording_{0}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"));
            Buffer.BlockCopy(byteSamples, 0, samples, 0, byteSamples.Length);
            audioBuffer.Dispose();
            audioBuffer = null;
            EventUtility.Dispatch(() => {
                var audioClip = AudioClip.Create(
                    recordingName,
                    totalSampleCount / Format.channelCount,
                    Format.channelCount,
                    Format.sampleRate,
                    false
                );
                audioClip.SetData(samples, 0);
                Callback(audioClip);
            });
        }
        #endregion
    }
}