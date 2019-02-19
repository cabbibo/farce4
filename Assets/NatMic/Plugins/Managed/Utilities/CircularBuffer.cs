/* 
*   NatMic
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatMicU.Utilities {

    using UnityEngine;
    using System;
    using System.Runtime.CompilerServices;

    public sealed class CircularBuffer {

        #region --Op vars--
        public readonly float[] Buffer;
        public int Size { get { return Buffer.Length; }}
        private int totalWriteCount, totalReadCount;
        #endregion


        #region --Client API--

        public CircularBuffer (int size = 1 << 16) {
            Buffer = new float[size];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Write (float[] src) {
            if (src == null)
                throw new NullReferenceException("Source buffer must not be null");
            var index = totalWriteCount % Buffer.Length;
            var remaining = Buffer.Length - index;
            var copyCount = Mathf.Min(src.Length, remaining);
            var residualCount = src.Length - copyCount;
            Array.Copy(src, 0, Buffer, index, copyCount);
            Array.Copy(src, copyCount, Buffer, 0, residualCount);
            totalWriteCount += src.Length;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Read (float[] dst, int count) {
            if (dst == null)
                throw new NullReferenceException("Destination buffer must not be null");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count, "Count must be non-negative");
            if (dst.Length < count)
                throw new ArgumentException("Destination buffer must be as big as read count", "count");
            var index = totalReadCount % Buffer.Length;
            var remaining = Buffer.Length - index;
            var copyCount = Mathf.Min(count, remaining);
            var residualCount = count - copyCount;
            Array.Copy(Buffer, index, dst, 0, copyCount);
            Array.Copy(Buffer, 0, dst, copyCount, residualCount);
            totalReadCount += count;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Advance (int count) {
            totalReadCount += count;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool CanRead (int count) {
            return totalWriteCount - totalReadCount >= count;
        }
        #endregion
    }
}