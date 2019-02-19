/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorderU.Core.Recorders {

    using UnityEngine;
    using System;
    using Clocks;
    using Docs;
    using NatRenderU.Dispatch;

    /// <summary>
    /// Recorder for recording a game camera
    /// </summary>
    [Doc(@"CameraRecorder")]
    public class CameraRecorder : IRecorder {
        
        #region --Op vars--
        /// <summary>
        /// Control number of successive camera frames to skip while recording.
        /// This is very useful for GIF recording, which typically has a lower framerate appearance
        /// </summary>
        [Doc(@"CameraRecorderNthFrame", @"CameraRecorderNthFrameDiscussion"), Code(@"RecordGIF")]
        public int recordEveryNthFrame = 1;
        protected readonly Camera Camera;
        protected readonly IClock Clock;
        protected int frameCount;
        #endregion


        #region --Client API--

        /// <summary>
        /// Create a camera recorder for a game camera
        /// </summary>
        /// <param name="camera">Game camera to record</param>
        /// <param name="clock">Optional. Clock for generating timestamps</param>
        [Doc(@"CameraRecorderCreate"), Code(@"RecordGIF")]
        public static CameraRecorder Create (Camera camera, IClock clock = null) {
            if (!camera) {
                Debug.LogError("NatCorder Error: Cannot create video recorder with no camera provided");
                return null;
            }
            return new CameraRecorder(camera, clock ?? new RealtimeClock());
        }

        /// <summary>
        /// Stop recording and teardown resources
        /// </summary>
        [Doc(@"CameraRecorderDispose")]
        public virtual void Dispose () {
            DispatchUtility.onFrame -= OnFrame;
        }
        #endregion


        #region --Operations--

        protected CameraRecorder (Camera camera, IClock clock) {
            Camera = camera;
            Clock = clock;
            DispatchUtility.onFrame += OnFrame;
        }

        private void OnFrame () {
            if (NatCorder.IsRecording && frameCount++ % recordEveryNthFrame == 0)
                RecordFrame();
        }

        protected virtual void RecordFrame () {
            // Acquire frame, save camera state
            var encoderFrame = NatCorder.AcquireFrame();
            var prevTarget = Camera.targetTexture;
            Camera.targetTexture = encoderFrame;               
            // Render
            Camera.Render();
            // Restore camera state, commit frame
            Camera.targetTexture = prevTarget;
            NatCorder.CommitFrame(encoderFrame, Clock.CurrentTimestamp);
        }
        #endregion
    }
}