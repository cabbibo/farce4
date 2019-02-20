/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Core.Platforms {

    using UnityEngine;
    using UnityEngine.Scripting;
    using System;

    public sealed class NatMicAndroid : AndroidJavaProxy, INatMic {

        #region --Op vars--
        private volatile SampleBufferCallback callback;
        private readonly AndroidJavaObject natmic;
        #endregion


        #region --Properties--

        public Device[] Devices {
            get {
                var count = natmic.Call<int>("deviceCount");
                var devices = new DeviceAndroid[count];
                for (var i = 0; i < count; i++)
                    devices[i] = new DeviceAndroid(natmic.Call<AndroidJavaObject>("acquireDevice", i));
                return devices;
            }
        }

        public bool IsRecording {
            get { return natmic.Call<bool>("isRecording"); }
        }

        public long CurrentTimestamp {
            get { return natmic.Call<long>("currentTimestamp"); }
        }

        public string RecordingPath { get; private set; }
        #endregion


        #region --Operations--

        public NatMicAndroid () : base("com.yusufolokoba.natmic.NatMicDelegate") {
            natmic = new AndroidJavaObject("com.yusufolokoba.natmic.NatMic", this);
            RecordingPath = Application.persistentDataPath;
            Debug.Log("NatMic: Initialized NatMic 1.2 Android backend");
        }

        public void StartRecording (Device device, Format format, SampleBufferCallback callback) {
            this.callback = callback;
            natmic.Call(
                "startRecording",
                (AndroidJavaObject)(device as DeviceAndroid),
                format.sampleRate,
                format.channelCount
            );
        }

        public void StopRecording () {
            natmic.Call("stopRecording");
            callback = null;
        }
        #endregion


        #region --Callbacks--

        [Preserve]
        private void onSampleBuffer (AndroidJavaObject frame) {
            if (callback == null)
                return;
            var sampleBuffer = AndroidJNI.FromFloatArray(frame.Get<AndroidJavaObject>("sampleBuffer").GetRawObject());
            var timestamp = frame.Get<long>("timestamp");
            try {
                callback(sampleBuffer, timestamp);
            } catch (Exception ex) {
                Debug.LogError("NatMic Error: Sample buffer callback raised exception: "+ex);
            }
        }
        #endregion
    }
}