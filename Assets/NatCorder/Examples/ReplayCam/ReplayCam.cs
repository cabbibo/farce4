/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorderU.Examples {

    #if UNITY_EDITOR
	using UnityEditor;
	#endif
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Core;
    using Core.Recorders;
    using Core.Clocks;

    public class ReplayCam : MonoBehaviour {

        /**
        * ReplayCam Example
        * -----------------
        * This example records the screen using a `CameraRecorder`.
        * When we want mic audio, we play the mic to an AudioSource and record the audio source using an `AudioRecorder`
        * -----------------
        * Note that UI canvases in Overlay mode cannot be recorded, so we use a different mode (this is a Unity issue)
        */

        [Header("Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;

        [Header("Microphone")]
        public bool recordMicrophone;
        public AudioSource microphoneSource;

        private CameraRecorder videoRecorder;
        private AudioRecorder audioRecorder;
        private IClock recordingClock;

        public void StartRecording () {
            // Create recording formats
            var videoFormat = new VideoFormat(videoWidth, videoHeight);
            var audioFormat = recordMicrophone ? AudioFormat.Unity : AudioFormat.None;
            // Create a recording clock for generating timestamps
            recordingClock = new RealtimeClock();
            // Start recording
            NatCorder.StartRecording(Container.MP4, videoFormat, audioFormat, OnReplay);
            videoRecorder = CameraRecorder.Create(Camera.main, recordingClock);
            // Start microphone and create audio recorder
            if (recordMicrophone) {
                StartMicrophone();
                audioRecorder = AudioRecorder.Create(microphoneSource, true, recordingClock);
            }
        }

        private void StartMicrophone () {
            #if !UNITY_WEBGL || UNITY_EDITOR // No `Microphone` API on WebGL :(
            // Create a microphone clip
            microphoneSource.clip = Microphone.Start(null, true, 60, 48000);
            while (Microphone.GetPosition(null) <= 0) ;
            // Play through audio source
            microphoneSource.timeSamples = Microphone.GetPosition(null);
            microphoneSource.loop = true;
            microphoneSource.Play();
            #endif
        }

        public void StopRecording () {
            // Stop the microphone if we used it for recording
            if (recordMicrophone) {
                StopMicrophone();
                audioRecorder.Dispose();
            }
            // Stop the recording
            videoRecorder.Dispose();
            NatCorder.StopRecording();
        }

        private void StopMicrophone () {
            #if !UNITY_WEBGL || UNITY_EDITOR
            Microphone.End(null);
            microphoneSource.Stop();
            #endif
        }

        private void OnReplay (string path) {
            Debug.Log("Saved recording to: "+path);
            // Playback the video
            #if UNITY_EDITOR
			EditorUtility.OpenWithDefaultApp(path);
            #elif UNITY_IOS
            Handheld.PlayFullScreenMovie("file://" + path);
            #elif UNITY_ANDROID
            Handheld.PlayFullScreenMovie(path);
            #endif
        }
    }
}