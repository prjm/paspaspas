using System;

namespace PasPasPas.Runtime.Common {

    /// <summary>
    ///     helper class for byte arrays
    /// </summary>
    public static class ByteArrayHelper {

        /// <summary>
        ///     create a bit array
        /// </summary>
        /// <param name="numberOfBits">number of bits</param>
        /// <param name="setAllBits">if <c>true</c>, all bits of the result value are set</param>
        /// <returns></returns>
        private static Bits CreateBits(int numberOfBits, bool setAllBits) {
            var result = new Bits(numberOfBits);
            if (setAllBits)
                result.Invert();
            return result;
        }

        /// <summary>
        ///     create an array of bits
        /// </summary>
        /// <param name="bits">bit array</param>
        /// <param name="isNegative">if <c>true</c>the result has to be interpreted as negative number</param>
        /// <param name="maxSize">max. number of bytes</param>
        /// <returns></returns>
        private static byte[] CreateByteArray(Bits bits, bool isNegative, int maxSize) {
            var index = bits.LastIndexOf(!isNegative);
            var numberOfElements = Math.Max(1, (index + 8 * sizeof(byte)) / (8 * sizeof(byte)));

            if (isNegative && index % 8 == 7)
                numberOfElements++;

            if (numberOfElements == 3)
                numberOfElements = 4;

            if (numberOfElements > 4)
                numberOfElements = maxSize;

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
        ///     converts a byte array to an unsigned long
        /// </summary>
        /// <param name="data">bytes to convert</param>
        /// <returns>unsigned long value of the byte array</returns>
        public static ulong ToUnsignedLong(byte[] data) {
            var bits = CreateBits(8 * data.Length, false);
            bits.AsByteArray = data;
            return bits.LeastSignificantQuadWord;
        }

        /// <summary>
        ///     converts a byte array to an signed long
        /// </summary>
        /// <param name="data">bytes to convert</param>
        /// <returns>unsigned long value of the byte array</returns>
        public static long ToSignedLong(byte[] data) {
            var bits = CreateBits(8 * data.Length, false);
            bits.AsByteArray = data;
            return bits.LeastSignificantSignedQuadWord;
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

        /// <summary>
        ///     creates a two complement of a given byte array
        /// </summary>
        /// <param name="byteSize">byte size to use</param>
        /// <param name="data">array to complement</param>
        /// <param name="isNegative"><c>true</c> if the data displays a negative number</param>
        /// <returns>negated number</returns>
        public static ByteArrayCalculation TwoComplement(int byteSize, bool isNegative, byte[] data) {
            var result = CreateBits(8 * byteSize + 1, isNegative);
            result.AsByteArray = data;
            result.TwoComplement();
            return new ByteArrayCalculation(result.MostSignificantBit, CreateByteArray(result, result.MostSignificantBit, byteSize));
        }

        /// <summary>
        ///     add two byte arrays arithmetically
        /// </summary>
        /// <returns>sum of addition and overflow status</returns>
        public static ByteArrayCalculation Add(int byteSize, ByteArrayCalculation augend, ByteArrayCalculation addend) {
            var result = CreateBits(1 + 8 * byteSize, augend.IsNegative);
            result.AsByteArray = augend.Data;

            var otherValue = CreateBits(1 + 8 * byteSize, addend.IsNegative);
            otherValue.AsByteArray = addend.Data;
            result.Add(otherValue);

            if (result[8 * byteSize] != result[result.Length - 2])
                return new ByteArrayCalculation(overflow: true);
            else
                return new ByteArrayCalculation(result.MostSignificantBit, CreateByteArray(result, result.MostSignificantBit, byteSize));
        }

        /// <summary>
        ///     multiply two byte arrays arithmetically
        /// </summary>
        /// <returns>multiplication result and overflow status</returns>
        public static ByteArrayCalculation Multiply(int byteSize, ByteArrayCalculation multplicand, ByteArrayCalculation multiplier) {
            var left = CreateBits(8 * byteSize, multplicand.IsNegative);
            left.AsByteArray = multplicand.Data;

            var right = CreateBits(8 * byteSize, multiplier.IsNegative);
            right.AsByteArray = multiplier.Data;

            var result = left.Multiply(right);

            if (result.LastIndexOf(!result.MostSignificantBit) >= 8 * byteSize)
                return new ByteArrayCalculation(overflow: true);
            else
                return new ByteArrayCalculation(result.MostSignificantBit, CreateByteArray(result, result.MostSignificantBit, byteSize));
        }

        /// <summary>
        ///     divide two byte arrays arithmetically
        /// </summary>
        /// <param name="numberOfBytes">number of bytes used</param>
        /// <param name="dividend">dividend</param>
        /// <param name="divisor">divisor</param>
        /// <returns>division result</returns>
        public static ByteArrayCalculation Divide(int numberOfBytes, ByteArrayCalculation dividend, ByteArrayCalculation divisor) {
            var left = CreateBits(8 * numberOfBytes, dividend.IsNegative);
            left.AsByteArray = dividend.Data;

            var right = CreateBits(8 * numberOfBytes, divisor.IsNegative);
            right.AsByteArray = divisor.Data;

            var result = left.Divide(right);
            return new ByteArrayCalculation(result.MostSignificantBit, CreateByteArray(result, result.MostSignificantBit, numberOfBytes));
        }

        /// <summary>
        ///     divide two byte arrays arithmetically and get the remainder
        /// </summary>
        /// <param name="numberOfBytes">number of bytes used</param>
        /// <param name="dividend">dividend</param>
        /// <param name="divisor">divisor</param>
        /// <returns>division result</returns>
        public static ByteArrayCalculation Modulo(int numberOfBytes, ByteArrayCalculation dividend, ByteArrayCalculation divisor) {
            var left = CreateBits(8 * numberOfBytes, dividend.IsNegative);
            left.AsByteArray = dividend.Data;

            var right = CreateBits(8 * numberOfBytes, divisor.IsNegative);
            right.AsByteArray = divisor.Data;

            var result = left.Modulo(right);
            return new ByteArrayCalculation(result.MostSignificantBit, CreateByteArray(result, result.MostSignificantBit, numberOfBytes));
        }

        /// <summary>
        ///     invert all bits of the given byte array
        /// </summary>
        /// <param name="numberOfBytes">number of bytes used</param>
        /// <param name="operand"></param>
        /// <returns>inverse bytes</returns>
        public static ByteArrayCalculation Not(int numberOfBytes, ByteArrayCalculation operand) {
            var result = CreateBits(8 * numberOfBytes, operand.IsNegative);
            result.AsByteArray = operand.Data;
            result.Invert();
            return new ByteArrayCalculation(result.MostSignificantBit, CreateByteArray(result, result.MostSignificantBit, numberOfBytes));
        }

        /// <summary>
        ///     bitwise and
        /// </summary>
        /// <param name="numberOfBytes"></param>
        /// <param name="leftOperand"></param>
        /// <param name="rightOperand"></param>
        /// <returns></returns>
        public static ByteArrayCalculation And(int numberOfBytes, ByteArrayCalculation leftOperand, ByteArrayCalculation rightOperand) {
            var left = CreateBits(8 * numberOfBytes, leftOperand.IsNegative);
            left.AsByteArray = leftOperand.Data;

            var right = CreateBits(8 * numberOfBytes, rightOperand.IsNegative);
            right.AsByteArray = rightOperand.Data;

            left.And(right);
            return new ByteArrayCalculation(left.MostSignificantBit, CreateByteArray(left, left.MostSignificantBit, numberOfBytes));
        }

        /// <summary>
        ///     bitwise or
        /// </summary>
        /// <param name="numberOfBytes"></param>
        /// <param name="leftOperand"></param>
        /// <param name="rightOperand"></param>
        /// <returns></returns>
        public static ByteArrayCalculation Or(int numberOfBytes, ByteArrayCalculation leftOperand, ByteArrayCalculation rightOperand) {
            var left = CreateBits(8 * numberOfBytes, leftOperand.IsNegative);
            left.AsByteArray = leftOperand.Data;

            var right = CreateBits(8 * numberOfBytes, rightOperand.IsNegative);
            right.AsByteArray = rightOperand.Data;

            left.Or(right);
            return new ByteArrayCalculation(left.MostSignificantBit, CreateByteArray(left, left.MostSignificantBit, numberOfBytes));
        }

        /// <summary>
        ///     bitwise xor
        /// </summary>
        /// <param name="numberOfBytes"></param>
        /// <param name="leftOperand"></param>
        /// <param name="rightOperand"></param>
        /// <returns></returns>
        public static ByteArrayCalculation Xor(int numberOfBytes, ByteArrayCalculation leftOperand, ByteArrayCalculation rightOperand) {
            var left = CreateBits(8 * numberOfBytes, leftOperand.IsNegative);
            left.AsByteArray = leftOperand.Data;

            var right = CreateBits(8 * numberOfBytes, rightOperand.IsNegative);
            right.AsByteArray = rightOperand.Data;

            left.Xor(right);
            return new ByteArrayCalculation(left.MostSignificantBit, CreateByteArray(left, left.MostSignificantBit, numberOfBytes));
        }

        /// <summary>
        ///     shift left
        /// </summary>
        /// <param name="numberOfBytes"></param>
        /// <param name="leftOperand"></param>
        /// <param name="rightOperand"></param>
        /// <returns></returns>
        public static ByteArrayCalculation Shl(int numberOfBytes, ByteArrayCalculation leftOperand, ByteArrayCalculation rightOperand) {
            var left = CreateBits(8 * numberOfBytes, leftOperand.IsNegative);
            left.AsByteArray = leftOperand.Data;

            var right = CreateBits(8 * numberOfBytes, rightOperand.IsNegative);
            right.AsByteArray = rightOperand.Data;

            int numberOfBits;

            if (numberOfBytes == 4)
                numberOfBits = right.LeastSignificantSignedByte & 31;
            else
                numberOfBits = right.LeastSignificantSignedByte & 63;

            left.ShiftLeft(numberOfBits);
            return new ByteArrayCalculation(left.MostSignificantBit, CreateByteArray(left, left.MostSignificantBit, numberOfBytes));
        }

        /// <summary>
        ///     shift left
        /// </summary>
        /// <param name="numberOfBytes"></param>
        /// <param name="leftOperand"></param>
        /// <param name="rightOperand"></param>
        /// <returns></returns>
        public static ByteArrayCalculation Shr(int numberOfBytes, ByteArrayCalculation leftOperand, ByteArrayCalculation rightOperand) {
            var left = CreateBits(8 * numberOfBytes, leftOperand.IsNegative);
            left.AsByteArray = leftOperand.Data;

            var right = CreateBits(8 * numberOfBytes, rightOperand.IsNegative);
            right.AsByteArray = rightOperand.Data;

            int numberOfBits;

            if (numberOfBytes == 4)
                numberOfBits = right.LeastSignificantSignedByte & 31;
            else
                numberOfBits = right.LeastSignificantSignedByte & 63;

            left.ShiftRight(numberOfBits);
            return new ByteArrayCalculation(left.MostSignificantBit, CreateByteArray(left, left.MostSignificantBit, numberOfBytes));
        }
    }
}