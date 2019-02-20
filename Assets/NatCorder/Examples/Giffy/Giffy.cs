/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorderU.Examples {

    #if UNITY_EDITOR
	using UnityEditor;
	#endif
    using UnityEngine;
    using Core;
    using Core.Recorders;

    public class Giffy : MonoBehaviour {
        
        [Header("GIF Settings")]
        public int imageWidth = 640;
        public int imageHeight = 480;

        private CameraRecorder recorder;

        public void StartRecording () {
            // Start recording
            var videoFormat = new VideoFormat(imageWidth, imageHeight, 10);
            NatCorder.StartRecording(Container.GIF, videoFormat, AudioFormat.None, OnGIF);
            // Create a camera recorder
            recorder = CameraRecorder.Create(Camera.main);
            // Get a real GIF look by skipping frames
            recorder.recordEveryNthFrame = 5;
        }

        public void StopRecording () {
            // Stop the recording
            recorder.Dispose();
            NatCorder.StopRecording();
        }

        private void OnGIF (string path) {
            Debug.Log("Saved recording to: "+path);
            // Playback the video
            #if UNITY_EDITOR
            EditorUtility.OpenWithDefaultApp(path);
            #elif UNITY_IOS
            Application.OpenURL("file://" + path);
            #elif UNITY_ANDROID
            Application.OpenURL(path);
            #endif
        }
    }
}