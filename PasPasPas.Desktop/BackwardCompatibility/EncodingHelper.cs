using System;
using System.Buffers;
using System.Text;

namespace PasPasPas.Desktop.BackwardCompatibility {
    public static class EncodingHelper {


        public static string GetString(this Encoding encoding, in Span<byte> input) {
            var sharedBuffer = ArrayPool<byte>.Shared.Rent(input.Length);
            try {
                input.CopyTo(sharedBuffer);
                return encoding.GetString(sharedBuffer, 0, input.Length);
            }
            finally {
                ArrayPool<byte>.Shared.Return(sharedBuffer);
            }
        }

        public static void GetBytes(this Encoding encoding, string input, in Span<byte> output) {
            var data = encoding.GetBytes(input);
            data.CopyTo(output);
        }

        public static void GetBytes(this Encoding encoding, string input, byte[] output) {
            var data = encoding.GetBytes(input);
            Array.Copy(data, output, input.Length);
        }

        public static void GetChars(this Encoding encoding, in Span<byte> input, in Span<char> output) {
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
