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
    /// Recorder for recording audio to WAV files
    /// </summary>
    [Doc(@"WAVRecorder")]
    public sealed class WAVRecorder : IRecorder {

        #region --Op vars--
        /// <summary>
        /// Format of audio data that will be recorded
        /// </summary>
        [Doc(@"IRecorderFormat")]
        public Format Format { get; private set; }
        private readonly Action<string> Callback;
        private FileStream outputStream;
        private int totalSamples;
        #endregion


        #region --Client API--

        /// <summary>
        /// Create a WAV recorder
        /// </summary>
        /// <param name="format">Audio data format</param>
        /// <param name="callback">Callback invoked once the recording is ready</param>
        [Doc(@"WAVRecorderCtor")]
        public WAVRecorder (Format format, Action<string> callback) {
            this.Format = format;
            this.Callback = callback;
        }

        /// <summary>
        /// Start recording
        /// </summary>
        [Doc(@"IRecorderStartRecording", @"IRecorderStartRecordingDescription")]
        public void StartRecording () {
            // Create filestream
            var filename = string.Format("recording_{0}.wav", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"));
            outputStream = new FileStream(Path.Combine(NatMic.Implementation.RecordingPath, filename), FileMode.Create);
            // 'Allocate' header
            const byte @null = new byte();
	        for (int i = 0; i < 44; i++) outputStream.WriteByte(@null);            
        }

        /// <summary>
        /// Commit audio data to be recorded
        /// </summary>
        /// <param name="samples">Audio sample buffer</param>
        /// <param name="timestamp">Timestamp for the sample buffer in nanoseconds</param>
        [Doc(@"IRecorderCommitSamples")]
        public void CommitSamples (float[] samples, long timestamp) {
            // State checking
            if (outputStream == null) {
                Debug.LogError("NatMic Error: WAVRecorder::StartRecording must be called before committing samples");
                return;
            }
            if (samples == null) {
                Debug.LogError("NatMic Error: WAVRecorder::CommitSamples called with null sample buffer");
                return;
            }
            // Convert to short array then write to byte buffer
            var shortBuffer = new short[samples.Length];
            var byteBuffer = new byte[Buffer.ByteLength(shortBuffer)];
            for (int i = 0; i < samples.Length; i++) 
                shortBuffer[i] = (short)(samples[i] * short.MaxValue);
            Buffer.BlockCopy(shortBuffer, 0, byteBuffer, 0, byteBuffer.Length);
            // Write
            outputStream.Write(byteBuffer, 0, byteBuffer.Length);
            totalSamples += samples.Length;
        }

        /// <summary>
        /// Stop writing and invoke recording callback
        /// </summary>
        [Doc(@"IRecorderDispose")]
        public void Dispose () {
            if (outputStream == null)
                return;
            WriteHeader();
            var filePath = outputStream.Name;
            outputStream.Dispose();
            EventUtility.Dispatch(() => Callback(filePath));
        }
        #endregion


        #region --Operations--

        private void WriteHeader () {
            outputStream.Seek(0, SeekOrigin.Begin);
            outputStream.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"), 0, 4);
            outputStream.Write(BitConverter.GetBytes(outputStream.Length - 8), 0, 4);
            outputStream.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"), 0, 4);
            outputStream.Write(System.Text.Encoding.UTF8.GetBytes("fmt "), 0, 4);
            outputStream.Write(BitConverter.GetBytes(16), 0, 4);
            outputStream.Write(BitConverter.GetBytes((ushort)1), 0, 2);
            outputStream.Write(BitConverter.GetBytes(Format.channelCount), 0, 2);                                       // Channel count
            outputStream.Write(BitConverter.GetBytes(Format.sampleRate), 0, 4);                                         // Sample rate
            outputStream.Write(BitConverter.GetBytes(Format.sampleRate * Format.channelCount * sizeof(short)), 0, 4);   // Output rate in bytes
            outputStream.Write(BitConverter.GetBytes((ushort)(Format.channelCount * 2)), 0, 2);                         // Block alignment
            outputStream.Write(BitConverter.GetBytes((ushort)16), 0, 2);                                                // Bits per sample
            outputStream.Write(System.Text.Encoding.UTF8.GetBytes("data"), 0, 4);
            outputStream.Write(BitConverter.GetBytes(totalSamples * sizeof(ushort)), 0, 4);                             // Total sample count
        }
        #endregion
    }
}