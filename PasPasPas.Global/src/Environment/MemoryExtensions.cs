using System;
using System.Buffers;
using System.IO;
using System.Text;

namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     memory helper for desktop platforms
    /// </summary>
    public static class MemoryExtensions {

        /// <summary>
        ///     helper functions for desktop framework
        /// </summary>
        /// <param name="thisStream"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static int ReadSpan(this Stream thisStream, in Span<byte> buffer, int len) {
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


        public static void WriteSpan(this Stream thisStream, in ReadOnlySpan<byte> buffer) {
            var sharedBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try {
                buffer.CopyTo(sharedBuffer);
                thisStream.Write(sharedBuffer, 0, buffer.Length);
            }
            finally {
                ArrayPool<byte>.Shared.Return(sharedBuffer);
            }
        }

        public static string Decode(this Encoding encoding, in Span<byte> input) {
            var sharedBuffer = ArrayPool<byte>.Shared.Rent(input.Length);
            try {
                input.CopyTo(sharedBuffer);
                return encoding.GetString(sharedBuffer, 0, input.Length);
            }
            finally {
                ArrayPool<byte>.Shared.Return(sharedBuffer);
            }
        }

        public static void Encode(this Encoding encoding, string input, in Span<byte> output) {
            var data = encoding.GetBytes(input);
            data.CopyTo(output);
        }

        public static void Encode(this Encoding encoding, string input, byte[] output) {
            var data = encoding.GetBytes(input);
            Array.Copy(data, output, input.Length);
        }

        public static void GetCharsBySpan(this Encoding encoding, in Span<byte> input, in Span<char> output) {
            var sharedBuffer = ArrayPool<byte>.Shared.Rent(input.Length);
            try {
                input.CopyTo(sharedBuffer);
                var data = encoding.GetChars(sharedBuffer);
                data.CopyTo(output);
            }
            finally {
                ArrayPool<byte>.Shared.Return(sharedBuffer);
            }

        }

    }
}
