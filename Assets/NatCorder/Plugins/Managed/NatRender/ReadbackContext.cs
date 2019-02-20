/* 
*   NatRender
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatRenderU {

    using UnityEngine;
    using System;
    using Readback;

    public abstract class ReadbackContext : IDisposable {

        #region --Ops--
        protected readonly TextureFormat format;
        protected readonly SynchronizationHint synchronizationHint;

        protected ReadbackContext (TextureFormat readbackFormat, SynchronizationHint synchronizationHint) {
            this.format = readbackFormat;
            this.synchronizationHint = synchronizationHint;
        }
        #endregion


        #region --Client API--

        /// <summary>
        /// Create a readback context
        /// </summary>
        public static ReadbackContext Create (TextureFormat readbackFormat, SynchronizationHint syncHint) {
            #if UNITY_EDITOR
            return new BlockingContext(readbackFormat, syncHint);
            #elif UNITY_IOS
            return new MTLContext(readbackFormat, syncHint); // We don't support GLES on iOS
            #elif UNITY_ANDROID
            return new GLESContext(readbackFormat, syncHint);
            #else
            return new BlockingContext(readbackFormat, syncHint);
            #endif
        }

        /// <summary>
        /// Dispose the fence and release resources
        /// </summary>
        public abstract void Dispose ();

        /// <summary>
        /// Readback pixel data from a RenderTexture into a pixel buffer.
        /// This function must be called on the Unity main thread.
        /// Note that the readback delegate may be invoked on any arbitrary thread.
        /// </summary>
        public abstract void Readback (RenderTexture frame, ReadbackDelegate handler);
        #endregion
    }
}