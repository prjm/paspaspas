using System;
using PasPasPas.Runtime.Common;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     helper class for byte arrays
    /// </summary>
    public static class ByteArrayHelper {

        private static Bits CreateBits(bool isNegative) {
            var result = new Bits(72);
            if (isNegative)
                result.Invert();
            return result;
        }

        private static byte[] CreateByteArray(Bits bits, bool isNegative) {
            var index = bits.LastIndexOf(!isNegative);
            var numberOfElements = Math.Max(1, (index + 8 * sizeof(byte)) / (8 * sizeof(byte)));

            if (isNegative && index % 8 == 7)
                numberOfElements++;

            if (numberOfElements == 3)
                numberOfElements = 4;

            if (numberOfElements > 4 && numberOfElements < 8)
                numberOfElements = 8;

            return bits.GetTrimmedByteArray(numberOfElements);
        }

        /// <summary>
        ///     create a byte array from a signed byte
        /// </summary>
        /// <param name="number">signed byte to convert</param>
        /// <returns>created byte array</returns>
        public static byte[] FromSignedByte(sbyte number) {
            return new byte[] { unchecked((byte)number) };
        }

        /// <summary>
        ///     create a byte array from a byte
        /// </summary>
        /// <param name="number">byte</param>
        /// <returns>byte array</returns>
        public static byte[] FromByte(byte number)
            => new byte[] { number };


        /// <summary>
        ///     create a byte array from a unsigned short
        /// </summary>
        /// <param name="number">short value to convert</param>
        public static byte[] FromUnsignedShort(ushort number) {
            var lsb = unchecked((byte)(number >> 0));
            var msb = unchecked((byte)(number >> 8));

            if (msb == 0)
                return new byte[] { lsb };

            return new byte[] { lsb, msb };
        }

        /// <summary>
        ///     create a byte array from a short
        /// </summary>
        /// <param name="number">short value to convert</param>
        public static byte[] FromShort(short number) {
            var lsb = unchecked((byte)(number >> 0));
            var msb = unchecked((byte)(number >> 8));

            if (msb == 0)
                return new byte[] { lsb };

            return new byte[] { lsb, msb };
        }

        /// <summary>
        ///     create a byte array from a unsigned integer
        /// </summary>
        /// <param name="number">int value to convert</param>
        public static byte[] FromUnsignedInteger(uint number) {
            var lo_lsb = unchecked((byte)(number >> 0));
            var lo_msb = unchecked((byte)(number >> 8));
            var hi_lsb = unchecked((byte)(number >> 16));
            var hi_msb = unchecked((byte)(number >> 24));

            if (hi_msb == 0 && hi_lsb == 0)
                if (lo_msb == 0)
                    return new byte[] { lo_lsb };
                else
                    return new byte[] { lo_lsb, lo_msb };
            else
                return new byte[] { lo_lsb, lo_msb, hi_lsb, hi_msb };
        }

        /// <summary>
        ///     create a byte array from a unsigned integer
        /// </summary>
        /// <param name="number">int value to convert</param>
        public static byte[] FromUnsignedLong(ulong number) {
            var lo_lo_lsb = unchecked((byte)(number >> 0));
            var lo_lo_msb = unchecked((byte)(number >> 8));
            var lo_hi_lsb = unchecked((byte)(number >> 16));
            var lo_hi_msb = unchecked((byte)(number >> 24));
            var hi_lo_lsb = unchecked((byte)(number >> 32));
            var hi_lo_msb = unchecked((byte)(number >> 40));
            var hi_hi_lsb = unchecked((byte)(number >> 48));
            var hi_hi_msb = unchecked((byte)(number >> 56));

            if (hi_hi_msb == 0 && hi_hi_lsb == 0 && hi_lo_msb == 0 && hi_lo_lsb == 0)
                if (lo_hi_msb == 0 && lo_hi_lsb == 0)
                    if (lo_lo_msb == 0)
                        return new byte[] { lo_lo_lsb };
                    else
                        return new byte[] { lo_lo_lsb, lo_lo_msb };
                else
                    return new byte[] { lo_lo_lsb, lo_lo_msb, lo_hi_lsb, lo_hi_msb };
            else
                return new byte[] { lo_lo_lsb, lo_lo_msb, lo_hi_lsb, lo_hi_msb, hi_lo_lsb, hi_lo_msb, hi_hi_lsb, hi_hi_msb };
        }

        /// <summary>
        ///     create a byte array from a unsigned integer
        /// </summary>
        /// <param name="number">int value to convert</param>
        public static byte[] FromInteger(int number)
            => FromUnsignedInteger(unchecked((uint)number));

        /// <summary>
        ///     create a byte array from a unsigned integer
        /// </summary>
        /// <param name="number">int value to convert</param>
        public static byte[] FromLong(long number)
            => FromUnsignedLong(unchecked((ulong)number));

    }
}

