/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using UnityEngine;
    using NatRenderU;

    public sealed class NatCorderWebGL : NatCorderiOS { // EXPERIMENTAL // Primarily because of Unity audio

        #region --Operations--

        public NatCorderWebGL () : base() {
            readbackFormat = TextureFormat.RGBA32;
            SyncHint = SynchronizationHint.LessMemory;
            Debug.Log("NatCorder: Initialized NatCorder 1.4 WebGL backend with iOS implementation");
        }
        #endregion
    }
}