/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorderU.Examples {

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Core;
    using Core.Clocks;

    public class GreyWorld : MonoBehaviour {

        /**
        * GreyWorld Example
        * ------------------
        * This example records a WebCamTexture
        * The WebCamTexture is recorded with a filter applied, using a shader/material
        * In `Update`, we blit the WebCamTexture to encoder surfaces (NatCorder.AcquireFrame) with the greyscale material/shader
        * When the user stops pressing the screen, we revert the greyness and stop recording
        */

        public RawImage rawImage;
        public CameraPreview cameraPreview;
        private float greyness;
        private IClock clock;
        private const float GreySpeed = 3f;

        void Update () {
            // Animate the greyness
            if (cameraPreview.cameraTexture && rawImage.texture == cameraPreview.cameraTexture) {
                var currentGreyness = rawImage.material.GetFloat("_Greyness");
                var targetGreyness = Mathf.Lerp(currentGreyness, greyness, GreySpeed * Time.deltaTime);
                rawImage.material.SetFloat("_Greyness", targetGreyness);
            }
            // Record frames
            if (NatCorder.IsRecording && cameraPreview.cameraTexture.didUpdateThisFrame) {
                var frame = NatCorder.AcquireFrame();
                Graphics.Blit(cameraPreview.cameraTexture, frame, rawImage.material);
                NatCorder.CommitFrame(frame, clock.CurrentTimestamp);
            }
        }

        public void StartRecording () {
            // Become grey
            greyness = 1f;
            // If the camera is in a potrait rotation, then we swap the width and height for recording
            bool isPortrait = cameraPreview.cameraTexture.videoRotationAngle == 90 || cameraPreview.cameraTexture.videoRotationAngle == 270;
            int recordingWidth = isPortrait ? cameraPreview.cameraTexture.height : cameraPreview.cameraTexture.width;
            int recordingHeight = isPortrait ? cameraPreview.cameraTexture.width : cameraPreview.cameraTexture.height;
            var videoFormat = new VideoFormat(recordingWidth, recordingHeight);
            clock = new RealtimeClock();
            // Start recording
            NatCorder.StartRecording(Container.MP4, videoFormat, AudioFormat.None, OnVideo); // DEBUG
        }

        public void StopRecording () {
            // Revert to normal color
            greyness = 0f;
            // Stop recording
            NatCorder.StopRecording();
        }

        void OnVideo (string path) {
            Debug.Log("Saved recording to: "+path);
            // Playback the video
            #if UNITY_IOS
            Handheld.PlayFullScreenMovie("file://" + path);
            #elif UNITY_ANDROID
            Handheld.PlayFullScreenMovie(path);
            #endif
        }
    }
}