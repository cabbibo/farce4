/* 
*   NatRender
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatRenderU.Readback {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using Dispatch;

    public sealed class MTLContext : ReadbackContext {

        #region --Op vars--
        private IntPtr pixelBuffer;
        #pragma warning disable 0414
        private int bufferSize;
        #pragma warning restore 0414
        private static Dispatcher delegateDispatcher; // Jump off Metal thread
        #endregion


        #region --ReadbackContext--

        public MTLContext (TextureFormat format, SynchronizationHint syncHint) : base(format, syncHint) {
            pixelBuffer = Marshal.AllocHGlobal(bufferSize = 16);
            delegateDispatcher = delegateDispatcher ?? Dispatcher.Create(DispatchThread.WorkerThread);
        }

        public override void Dispose () {
            Readback(null, ptr => {
                Marshal.FreeHGlobal(pixelBuffer);
                pixelBuffer = IntPtr.Zero;
            });
        }
        
        public override void Readback (RenderTexture frame, ReadbackDelegate handler) {
            #if UNITY_IOS
            var mtlTexture = frame ? frame.GetNativeTexturePtr() : IntPtr.Zero;
            var frameSize = frame ? frame.width * frame.height * 4 : 0;
            var handlerPtr = (IntPtr)GCHandle.Alloc(handler, GCHandleType.Normal);
            if (bufferSize != frameSize && frameSize > 0)
                pixelBuffer = Marshal.ReAllocHGlobal(pixelBuffer, (IntPtr)(bufferSize = frameSize));
            Readback(synchronizationHint, mtlTexture, pixelBuffer, OnReadback, handlerPtr);
            #endif
        }
        #endregion


        #region --Operations--

        private delegate void NativeReadbackDelegate (IntPtr handlerPtr, IntPtr bufferPtr);

        #if UNITY_IOS
        [DllImport("__Internal", EntryPoint = "NRMTLReadback")]        
        private static extern void Readback (SynchronizationHint syncHint, IntPtr srcTexture, IntPtr dstBuffer, NativeReadbackDelegate handler, IntPtr handlerContext);
        #endif

        [MonoPInvokeCallback(typeof(NativeReadbackDelegate))]
        private static void OnReadback (IntPtr bufferPtr, IntPtr handlerPtr) {
            GCHandle handle = (GCHandle)(IntPtr)handlerPtr;
            ReadbackDelegate target = handle.Target as ReadbackDelegate;
            handle.Free();
            if (target == null)
                return;
            delegateDispatcher.Dispatch(() => target(bufferPtr));
        }
        #endregion
    }
}