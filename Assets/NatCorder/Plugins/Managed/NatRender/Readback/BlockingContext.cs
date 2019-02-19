/* 
*   NatRender
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatRenderU.Readback {

    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;

    public sealed class BlockingContext : ReadbackContext {

        #region --Op vars--
        private readonly Texture2D framebuffer;
        private IntPtr pixelBuffer;
        private readonly Dispatcher dispatcher;
        #endregion
        

        #region --ReadbackContext--

        public BlockingContext (TextureFormat format, SynchronizationHint syncHint) : base(format, syncHint) {
            framebuffer = new Texture2D(16, 16, format, false, false);
            pixelBuffer = Marshal.AllocHGlobal(16);
            dispatcher = Dispatcher.Create(DispatchThread.MainThread);
        }

        public override void Dispose () {
            Readback(null, ptr => {
                Texture2D.Destroy(framebuffer);
                Marshal.FreeHGlobal(pixelBuffer);
                pixelBuffer = IntPtr.Zero;
                dispatcher.Dispose();
            });
        }
        
        public override void Readback (RenderTexture frame, ReadbackDelegate handler) {
            if (synchronizationHint == SynchronizationHint.LessMemory)
                handler(Readback(frame));
            else
                dispatcher.Dispatch(() => dispatcher.Dispatch(() => handler(Readback(frame))));
        }
        #endregion


        #region --Operations--

        private IntPtr Readback (RenderTexture frame) {
            // Null checking
            if (!frame)
                return IntPtr.Zero;
            // State checking
            if (framebuffer.width != frame.width || framebuffer.height != frame.height) {
                framebuffer.Resize(frame.width, frame.height);
                pixelBuffer = Marshal.ReAllocHGlobal(pixelBuffer, (IntPtr)(frame.width * frame.height * 4));
            }
            // Readback
            var currentRT = RenderTexture.active;
            RenderTexture.active = frame;
            framebuffer.ReadPixels(new Rect(0, 0, framebuffer.width, framebuffer.height), 0, 0, false);
            RenderTexture.active = currentRT;
            // Copy
            var data = framebuffer.GetRawTextureData();
            Marshal.Copy(data, 0, pixelBuffer, data.Length);
            return pixelBuffer;
        }
        #endregion
    }
}