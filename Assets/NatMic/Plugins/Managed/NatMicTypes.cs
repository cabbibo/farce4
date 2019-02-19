/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core {

    using UnityEngine;
    using System;
    using Docs;

    #region --Delegates--
    /// <summary>
    /// Delegate invoked when microphone reports a new sample buffer
    /// </summary>
    [Doc(@"SampleBufferCallback")]
    public delegate void SampleBufferCallback (float[] sampleBuffer, long timestamp);
    #endregion


    #region --Value Types--

    /// <summary>
    /// Value type used to specify microphone configuration
    /// </summary>
    [Doc(@"Format")]
    public struct Format : IEquatable<Format> {
        /// <summary>
        /// Audio sample rate
        /// </summary>
        [Doc(@"SampleRate")]
        public readonly int sampleRate;
        /// <summary>
        /// Audio channel count
        /// </summary>
        [Doc(@"ChannelCount")]
        public readonly int channelCount;
        /// <summary>
        /// Default microphone format
        /// </summary>
        [Doc(@"FormatDefault")]
        public static Format Default {
            get {
                return new Format(AudioSettings.outputSampleRate);
            }
        }
        /// <summary>
        /// Format when mixing with Unity audio
        /// </summary>
        [Doc(@"FormatUnity")]
        public static Format Unity {
            get {
                return new Format(AudioSettings.outputSampleRate, (int)AudioSettings.speakerMode);
            }
        }
        /// <summary>
        /// Create a microphone format
        /// </summary>
        /// <param name="sampleRate">Sample rate</param>
        /// <param name="channelCount">Channel count</param>
        [Doc(@"FormatCtor")]
        public Format (int sampleRate, int channelCount = 1) {
            this.sampleRate = sampleRate;
            this.channelCount = channelCount;
        }

        public override string ToString () {
            return "{ " + string.Format("{0}@{1}Hz", channelCount, sampleRate) + " }";
        }
        public bool Equals (Format other) {
            return other.channelCount == channelCount && other.sampleRate == sampleRate;
        }
        public override int GetHashCode () {
            return sampleRate ^ channelCount;
        }
        public override bool Equals (object obj) {
            return (obj is Format) && this.Equals((Format)obj);
        }
        public static bool operator == (Format lhs, Format rhs) {
            return lhs.Equals(rhs);
        }
        public static bool operator != (Format lhs, Format rhs) {
            return !lhs.Equals(rhs);
        }
    }
    #endregion
}