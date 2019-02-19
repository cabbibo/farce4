/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using UnityEngine;
    using NatRenderU;

    public sealed class NatCorderOSX : NatCorderiOS {

        #region --Operations--

        public NatCorderOSX () : base() {
            recordingDirectory =
            #if UNITY_EDITOR
            System.IO.Directory.GetCurrentDirectory();
            #else
            Application.persistentDataPath;
            #endif
            readbackFormat = TextureFormat.ARGB32;
            SyncHint = SynchronizationHint.LessFrameTime;
            Debug.Log("NatCorder: Initialized NatCorder 1.4 macOS backend with iOS implementation");
        }
        #endregion
    }
}