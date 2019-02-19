/* 
*   NatRender
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatRenderU {

    using System;
    using Dispatch;

    public abstract class Dispatcher : IDisposable {

        #region --Client API--

        public static Dispatcher Create (DispatchThread thread) {
            switch (thread) {
                case DispatchThread.MainThread:
                    return new MainDispatch();
                case DispatchThread.WorkerThread:
                    return new ConcurrentDispatch();
                case DispatchThread.RenderThread:
                    return new RenderDispatch();
                default:
                    return null;
            }
        }

        public abstract void Dispatch (Action workload);

        public abstract void Dispose ();
        #endregion
    }
}