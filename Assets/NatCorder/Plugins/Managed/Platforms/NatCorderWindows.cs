/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using UnityEngine;
    using NatRenderU;

    public sealed class NatCorderWindows : NatCorderiOS {
        
        #region --Operations--

        public NatCorderWindows () : base() {
            recordingDirectory =
            #if UNITY_EDITOR
            System.IO.Directory.GetCurrentDirectory();
            #else
            Application.persistentDataPath;
            #endif
            readbackFormat = TextureFormat.RGBA32;
            SyncHint = SynchronizationHint.LessFrameTime;
            Debug.Log("NatCorder: Initialized NatCorder 1.4 Windows backend with iOS implementation");
        }

        public override void StartRecording (Container container, VideoFormat videoFormat, AudioFormat audioFormat, RecordingCallback recordingCallback) {
            base.StartRecording(
                container,
                videoFormat,
                audioFormat,
                path => recordingCallback(path.Replace('/', '\\'))
            );
        }
        #endregion
    }
}