/* 
*   NatRender
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatRenderU.Readback {

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Dispatch;

    public sealed class GLESContext : ReadbackContext {

        #region --Op vars--
        private volatile Action completionHandler;
        private readonly Dispatcher dispatcher;
        private readonly AndroidJavaObject GPUFence;
        private readonly List<PendingReadback> queue = new List<PendingReadback>();
        #endregion


        #region --ReadbackContext--

        public GLESContext (TextureFormat format, SynchronizationHint syncHint) : base(format, syncHint) {
            dispatcher = Dispatcher.Create(DispatchThread.RenderThread);
            GPUFence = new AndroidJavaClass("com.yusufolokoba.natrender.GPUFence");
            DispatchUtility.onFrame += OnFrame;
        }

        public override void Dispose () {
            Readback(null, ptr => DispatchUtility.onFrame -= OnFrame);
        }

        public override void Readback (RenderTexture frame, ReadbackDelegate handler) {
            var textureId = frame ? frame.GetNativeTexturePtr() : IntPtr.Zero;
            dispatcher.Dispatch(() => {
                var fence = (IntPtr)GPUFence.CallStatic<long>("create", (int)synchronizationHint);
                if (GPUFence.CallStatic<bool>("complete", fence.ToInt64()))
                    handler(textureId);
                else queue.Add(new PendingReadback{
                    texture = textureId,
                    fence = fence,
                    handler = handler
                });
            });
        }
        #endregion


        #region --Operations--

        struct PendingReadback { public IntPtr texture, fence; public ReadbackDelegate handler; }

        void OnFrame () {
            dispatcher.Dispatch(() => {
                for (var i = queue.Count - 1; i >= 0; i--) {
                    var readback = queue[i];
                    if (GPUFence.CallStatic<bool>("complete", readback.fence.ToInt64())) {
                        queue.RemoveAt(i);
                        readback.handler(readback.texture);
                    }
                }
            });
        }
        #endregion
    }
}