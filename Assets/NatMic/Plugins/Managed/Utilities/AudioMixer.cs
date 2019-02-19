/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Utilities {

    using UnityEngine;
    using System;
    using System.Threading;
    using Core;

    public sealed class AudioMixer : IDisposable {

        #region --Op vars--
        private readonly SampleBufferCallback callback;
        private readonly Format format;
        private readonly CircularBuffer micBuffer, unityBuffer;
        private readonly Thread mixerThread;
        private long earliestTimestamp;
        private float[] unityStagingBuffer, micStagingBuffer, mixedBuffer;
        private bool running = true;
        private bool firstMicFrame = true, firstUnityFrame = true;
        private readonly object fence = new object();
        private const int SampleCount = 1024;
        #endregion

        
        #region --Client API--

        public AudioMixer (Format format, SampleBufferCallback callback) {
            // Set format
            this.format = format;
            this.callback = callback;
            // Create buffers
            micBuffer = new CircularBuffer();
            unityBuffer = new CircularBuffer();
            mixedBuffer = new float[SampleCount * format.channelCount];
            // Start thread
            mixerThread = new Thread(MixerLoop);
        }

        public void Dispose () {
            lock (fence) running = false;
        }
        #endregion


        #region --Operations--

        public void OnMicrophoneSampleBuffer (float[] sampleBuffer, long timestamp) {
            if (firstMicFrame) {
                Debug.Log("Mic start time: "+timestamp);
                lock (fence)
                    if (earliestTimestamp > 0) {
                        // Unity started first
                        var timeDelta = timestamp - (long)earliestTimestamp;
                        var skipCount = (int)(this.format.sampleRate * (Math.Abs(timeDelta) / 1e+9));
                        unityBuffer.Advance(skipCount * this.format.channelCount);
                        // Start mixer
                        mixerThread.Start();
                    }
                    else earliestTimestamp = timestamp;
                firstMicFrame = false;
            }
            micBuffer.Write(sampleBuffer);
        }

        public void OnUnitySampleBuffer (float[] sampleBuffer, long timestamp) {
            if (firstUnityFrame) {
                Debug.Log("Unity start time: "+timestamp);
                lock (fence)
                    if (earliestTimestamp > 0) {
                        // Microphone started first
                        var timeDelta = timestamp - (long)earliestTimestamp;
                        var skipCount = (int)(this.format.sampleRate * (Math.Abs(timeDelta) / 1e+9));
                        micBuffer.Advance(skipCount * this.format.channelCount);
                        // Start mixer
                        mixerThread.Start();
                    }
                    else earliestTimestamp = timestamp;
                firstUnityFrame = false;
            }
            unityBuffer.Write(sampleBuffer);
        }

        private void MixerLoop () {
            // Create buffers
            unityStagingBuffer = new float[SampleCount * format.channelCount];
            micStagingBuffer = new float[SampleCount * format.channelCount];
            // Loop
            while (true) {
                // Sleep
                Thread.Sleep(3);
                // Check
                lock (fence) if (!running) break;
                // Check that there are enough samples for this iteration
                if (!unityBuffer.CanRead(unityStagingBuffer.Length)) continue;
                if (!micBuffer.CanRead(micStagingBuffer.Length)) continue;
                // Read
                unityBuffer.Read(unityStagingBuffer, unityStagingBuffer.Length);
                micBuffer.Read(micStagingBuffer, micStagingBuffer.Length);
                // Mix
                AudioUtility.Mix(unityStagingBuffer, micStagingBuffer, mixedBuffer);
                callback(mixedBuffer, NatMic.Implementation.CurrentTimestamp);
            }
        }
        #endregion
    }
}