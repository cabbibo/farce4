/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core {

	using UnityEngine;
	using Platforms;
    using Utilities;
    using Docs;

    [Doc(@"NatMic")]
	public static class NatMic {
		
		#region --Properties--
        /// <summary>
        /// The backing implementation NatMic uses on this platform
        /// </summary>
        [Doc(@"Implementation")]
		public static readonly INatMic Implementation;
        /// <summary>
        /// Currently available audio capture devices
        /// </summary>
        [Doc(@"Devices")]
        public static Device[] Devices { get { return Implementation.Devices; }}
        /// <summary>
        /// Is the microphone running?
        /// </summary>
        [Doc(@"IsRecording")]
		public static bool IsRecording { get { return Implementation.IsRecording; }}
        #endregion


        #region --Op vars--
        private static AudioUtility audioUtility;
        private static AudioMixer audioMixer;
		#endregion


		#region --Operations--

        /// <summary>
        /// Start the microphone
        /// </summary>
        /// <param name="device">Recording device</param>
        /// <param name="format">Requested microphone format</param>
        /// <param name="sampleBufferCallback">Sample buffer callback</param>
        [Doc(@"StartRecording")]
		public static void StartRecording (Device device, Format format, SampleBufferCallback sampleBufferCallback) {
            if (IsRecording) {
                Debug.LogError("NatMic Error: Cannot start recording because NatMic is recording"); 
                return;
            }
            if (device == null) {
                Debug.LogError("NatMic Error: Cannot start recording from null device");
                return;
            }
            if (sampleBufferCallback == null) {
                Debug.LogError("NatMic Error: Cannot start recording with null callback");
                return;
            }
			Implementation.StartRecording(device, format, sampleBufferCallback);
		}

        /// <summary>
        /// Start the microphone with audio overlay from an audio source
        /// </summary>
        /// <param name="device">Recording device</param>
        /// <param name="audioSource">Audio source for audio overlay</param>
        /// <param name="sampleBufferCallback">Sample buffer callback</param>
        [Doc(@"StartRecordingAudioSource")]
        public static void StartRecording (Device device, AudioSource audioSource, SampleBufferCallback sampleBufferCallback) {
            if (IsRecording) {
                Debug.LogError("NatMic Error: Cannot start recording because NatMic is recording"); 
                return;
            }
            if (device == null) {
                Debug.LogError("NatMic Error: Cannot start recording from null device");
                return;
            }
            if (!audioSource) {
                Debug.LogError("NatMic Error: Cannot start recording because audio source is null"); 
                return;
            }
            if (sampleBufferCallback == null) {
                Debug.LogError("NatMic Error: Cannot start recording with null callback");
                return;
            }
            audioMixer = new AudioMixer(Format.Unity, sampleBufferCallback);
            audioUtility = AudioUtility.Create(audioSource.gameObject, audioMixer.OnUnitySampleBuffer);
            Implementation.StartRecording(device, Format.Unity, audioMixer.OnMicrophoneSampleBuffer);
        }

        /// <summary>
        /// Start the microphone with audio overlay from an audio listener
        /// </summary>
        /// <param name="device">Recording device</param>
        /// <param name="audioListener">Audio listener for audio overlay</param>
        /// <param name="sampleBufferCallback">Sample buffer callback</param>
        [Doc(@"StartRecordingAudioListener")]
        public static void StartRecording (Device device, AudioListener audioListener, SampleBufferCallback sampleBufferCallback) {
            if (IsRecording) {
                Debug.LogError("NatMic Error: Cannot start recording because NatMic is recording"); 
                return;
            }
            if (device == null) {
                Debug.LogError("NatMic Error: Cannot start recording from null device");
                return;
            }
            if (!audioListener) {
                Debug.LogError("NatMic Error: Cannot start recording because audio listener is null"); 
                return;
            }
            if (sampleBufferCallback == null) {
                Debug.LogError("NatMic Error: Cannot start recording with null callback");
                return;
            }
            audioMixer = new AudioMixer(Format.Unity, sampleBufferCallback);
            audioUtility = AudioUtility.Create(audioListener.gameObject, audioMixer.OnUnitySampleBuffer);
            Implementation.StartRecording(device, Format.Unity, audioMixer.OnMicrophoneSampleBuffer);
        }

        /// <summary>
        /// Stop the microphone
        /// </summary>
        [Doc(@"StopRecording")]
		public static void StopRecording () {
            if (!IsRecording) {
                Debug.LogError("NatMic Error: Cannot stop recording because NatMic is not recording");
                return;
            }
            if (audioUtility) audioUtility.Dispose();
            if (audioMixer != null) audioMixer.Dispose();
            audioUtility = null;
            audioMixer = null;
			Implementation.StopRecording();
		}
		#endregion


		#region --Initializer--

        static NatMic () {
            // Create implementation for this platform
            Implementation = 
            #if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            new NatMicOSX();
            #elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            new NatMicWindows();
            #elif UNITY_IOS
            new NatMiciOS();
            #elif UNITY_ANDROID
            new NatMicAndroid();
            #else
            new NatMicNull();
            #endif
            // Initialize event utility
            EventUtility.Initialize();
        }
        #endregion
	}
}