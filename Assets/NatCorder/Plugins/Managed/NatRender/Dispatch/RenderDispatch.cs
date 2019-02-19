/* 
*   NatRender
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatRenderU.Dispatch {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using Queue = System.Collections.Generic.Queue<System.Action>;

    public class RenderDispatch : Dispatcher { // Doesn't work in the Editor for some reason :/

        #region --Op vars--
        private readonly Queue queue = new Queue();
        private readonly object queueLock = new object();
        private readonly IntPtr weakSelf;
        private static readonly IntPtr delegateHandle;
        private static readonly UnityRenderingEvent renderingEvent;
        #endregion


        #region --Dispatcher--

        public RenderDispatch () : base() {
            weakSelf = (IntPtr)GCHandle.Alloc(this, GCHandleType.Weak);
            #if UNITY_ANDROID && !UNITY_EDITOR
            Dispatch(() => AndroidJNI.AttachCurrentThread());
            #endif
        }

        public override void Dispatch (Action action) {
            lock (queueLock)
				queue.Enqueue(action);
            GL.IssuePluginEvent(delegateHandle, weakSelf.ToInt32());
		}

		public override void Dispose () {} // Nop
        #endregion


        #region --Operations--

        private void Update () {
            lock (queueLock)
                queue.Dequeue()();
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void UnityRenderingEvent (int eventID);

        [MonoPInvokeCallback(typeof(UnityRenderingEvent))]
        private static void OnRenderEvent (int managedInstance) {
            GCHandle handle = (GCHandle)(IntPtr)managedInstance;
            RenderDispatch target = handle.Target as RenderDispatch;
            if (target == null)
                return;
            target.Update();
        }

        static RenderDispatch () {
            renderingEvent = OnRenderEvent;
            delegateHandle = Marshal.GetFunctionPointerForDelegate(renderingEvent);
        }
        #endregion
    }
}