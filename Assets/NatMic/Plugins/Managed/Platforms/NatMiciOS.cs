/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core.Platforms {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;

    public class NatMiciOS : INatMic {

        #region --Op vars--
        private SampleBufferCallback callback;
        private static NatMiciOS instance { get { return NatMic.Implementation as NatMiciOS; }}
        #endregion


        #region --Properties--

        public virtual Device[] Devices {
            get {
                var count = NatMicBridge.DeviceCount();
                var devices = new DeviceiOS[count];
                for (var i = 0; i < count; i++)
                    devices[i] = new DeviceiOS(NatMicBridge.AcquireDevice(i));
                return devices;
            }
        }

        public bool IsRecording {
            get { return NatMicBridge.IsRecording(); }
        }

        public long CurrentTimestamp {
            get { return NatMicBridge.CurrentTimestamp(); }
        }

        public string RecordingPath { get; protected set; }
        #endregion


        #region --Operations--

        public NatMiciOS () {
            RecordingPath = Application.persistentDataPath;
            Debug.Log("NatMic: Initialized NatMic 1.2 iOS backend");
        }

        public void StartRecording (Device device, Format format, SampleBufferCallback callback) {
            this.callback = callback;
            NatMicBridge.StartRecording(
                OnSampleBuffer,
                (IntPtr)(device as DeviceiOS),
                format.sampleRate,
                format.channelCount
            );
        }

        public void StopRecording () {
            NatMicBridge.StopRecording();
            callback = null;
        }
        #endregion


        #region --Callbacks--

        [MonoPInvokeCallback(typeof(NatMicBridge.AudioDataCallback))]
        private static void OnSampleBuffer (IntPtr sampleBuffer, int sampleCount, long timestamp) {
            if (instance.callback == null)
                return;
            float[] samples = new float[sampleCount];
            Marshal.Copy(sampleBuffer, samples, 0, sampleCount);
            try {
                instance.callback(samples, timestamp);
            } catch (Exception ex) {
                Debug.LogError("NatMic Error: Sample buffer callback raised exception: "+ex);
            }
        }
        #endregion
    }
}