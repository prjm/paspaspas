#nullable disable
using System;
using System.Buffers;
using System.IO;

namespace PasPasPas.Desktop.BackwardCompatibility {
    public static class StreamHelper {

        /// <summary>
        ///     helper functions for desktop framework
        /// </summary>
        /// <param name="thisStream"></param>
        /// <param name="buffer"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int Read(this Stream thisStream, in Span<byte> buffer) {
            var len = buffer.Length;
            var sharedBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try {
                var numRead = thisStream.Read(sharedBuffer, 0, len);
                if ((uint)numRead > (uint)len) {
                    throw new IOException();
                }
                new Span<byte>(sharedBuffer, 0, numRead).CopyTo(buffer);
                return numRead;
            }
            finally {
                ArrayPool<byte>.Shared.Return(sharedBuffer);
            }
        }

        /// <summary>
        ///     write a span
        /// </summary>
        /// <param name="thisStream"></param>
        /// <param name="buffer"></param>
        public static void Write(this Stream thisStream, in ReadOnlySpan<byte> buffer) {
            var sharedBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try {
                buffer.CopyTo(sharedBuffer);
                thisStream.Write(sharedBuffer, 0, buffer.Length);
            }
            finally {
                ArrayPool<byte>.Shared.Return(sharedBuffer);
            }
        }


    }
}
