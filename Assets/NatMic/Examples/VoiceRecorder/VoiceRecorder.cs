/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Examples {

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Core;
    using Core.Recorders;

    public class VoiceRecorder : MonoBehaviour {
        
        public AudioSource audioSource;
        private IRecorder recorder;

        public void ToggleRecording (Text buttonText) { // Invoked by UI
            if (!NatMic.IsRecording) {
                var format = Format.Default;
                // Create a WAV recorder to record the audio to a file
                recorder = new WAVRecorder(format, OnAudioFile);
                recorder.StartRecording();
                // Start recording
                var device = Device.Default;
                NatMic.StartRecording(device, format, OnSampleBuffer);
                buttonText.text = @"Stop Recording";
                Debug.Log("Starting audio input device: "+device);
            } else {
                // Stop recording the WAV file and dispose the recorder
                recorder.Dispose();
                // Stop recording
                NatMic.StopRecording();
                buttonText.text = @"Start Recording";
            }
        }

        private void OnSampleBuffer (float[] sampleBuffer, long timestamp) {
            // Commit the sample buffer to the WAV recorder
            recorder.CommitSamples(sampleBuffer, timestamp);
        }

        private void OnAudioFile (string path) {
            // Log the path
            Debug.Log("Saved recording to: "+path);
            // Play the recording in the scene
            Application.OpenURL(path);
        }
    }
}