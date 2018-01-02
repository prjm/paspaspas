using System;
using System.Linq;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     integer value with variable byte length
    /// </summary>
    /// <remarks>internally implemented as 9-byte integer values</remarks>
    public class ScaledIntegerValue : ValueBase, IIntegerValue {

        private byte[] data = new byte[9];

        private static void FillArray(byte[] data, byte value) {
            for (var i = 0; i < data.Length; i++)
                data[i] = value;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">number value</param>
        public ScaledIntegerValue(sbyte number) {
            if (number < 0)
                FillArray(data, 0xFF);

            data[0] = (byte)number;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">number value</param>
        public ScaledIntegerValue(byte number)
            => data[0] = number;

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">number value</param>
        public ScaledIntegerValue(short number) {
            if (number < 0)
                FillArray(data, 0xFF);

            data[0] = (byte)(number & 0xFF);
            data[1] = (byte)((number >> 8) & 0xFF);
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(ushort number) {
            data[0] = (byte)(number & 0xFF);
            data[1] = (byte)((number >> 8) & 0xFF);
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(int number) {
            if (number < 0)
                FillArray(data, 0xFF);

            data[0] = (byte)(number & 0xFF);
            data[1] = (byte)((number >> 8) & 0xFF);
            data[2] = (byte)((number >> 16) & 0xFF);
            data[3] = (byte)((number >> 24) & 0xFF);
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(uint number) {
            data[0] = (byte)(number & 0xFF);
            data[1] = (byte)((number >> 8) & 0xFF);
            data[2] = (byte)((number >> 16) & 0xFF);
            data[3] = (byte)((number >> 24) & 0xFF);
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(long number) {
            if (number < 0)
                FillArray(data, 0xFF);

            data[0] = (byte)(number & 0xFF);
            data[1] = (byte)((number >> 8) & 0xFF);
            data[2] = (byte)((number >> 16) & 0xFF);
            data[3] = (byte)((number >> 24) & 0xFF);
            data[4] = (byte)((number >> 32) & 0xFF);
            data[5] = (byte)((number >> 40) & 0xFF);
            data[6] = (byte)((number >> 48) & 0xFF);
            data[7] = (byte)((number >> 56) & 0xFF);
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(ulong number) {
            data[0] = (byte)(number & 0xFF);
            data[1] = (byte)((number >> 8) & 0xFF);
            data[2] = (byte)((number >> 16) & 0xFF);
            data[3] = (byte)((number >> 24) & 0xFF);
            data[4] = (byte)((number >> 32) & 0xFF);
            data[5] = (byte)((number >> 40) & 0xFF);
            data[6] = (byte)((number >> 48) & 0xFF);
            data[7] = (byte)((number >> 56) & 0xFF);
        }

        /// <summary>
        ///     create a new integer value for a given byte array
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(byte[] number) {
            if (number.Length != 9)
                throw new ArgumentOutOfRangeException(nameof(number));
            Array.Copy(number, data, 9);
        }

        /// <summary>
        ///     get the data of this value
        /// </summary>
        public override byte[] Data {
            get {
                var result = new byte[InternalLength];
                Array.Copy(data, result, result.Length);
                return result;
            }
        }

        /// <summary>
        ///     get the matching type
        /// </summary>
        public override int TypeId {
            get {
                var length = InternalLength;

                switch (length) {
                    case 0:
                        return KnownTypeIds.ShortInt;
                    case 1:
                        return IsNegative || data[0] < 0x80 ? KnownTypeIds.ShortInt : KnownTypeIds.ByteType;
                    case 2:
                        return IsNegative || data[1] < 0x80 ? KnownTypeIds.SmallInt : KnownTypeIds.WordType;
                    case 4:
                        return IsNegative || data[3] < 0x80 ? KnownTypeIds.IntegerType : KnownTypeIds.CardinalType;
                    case 8:
                        return IsNegative || data[7] < 0x80 ? KnownTypeIds.Int64Type : KnownTypeIds.Uint64Type;
                }
                throw new InvalidOperationException();
            }
        }

        private int InternalLength {
            get {
                int length;

                for (length = 8; length > 1; length--) {
                    if (IsNegative && data[length - 1] != 0xFF)
                        break;
                    if (!IsNegative && data[length - 1] != 0x00)
                        break;
                }

                if (IsNegative && data[length - 1] < 0x80)
                    length++;

                if (length > 4)
                    return 8;

                if (length > 2)
                    return 4;

                return length;
            }
        }

        /// <summary>
        ///     get the value as unsigned long
        /// </summary>
        public ulong AsUnsignedLong
            => BitConverter.ToUInt64(data, 0);

        /// <summary>
        ///     test if the number is negative
        /// </summary>
        public bool IsNegative
            => data[8] >= 0x80;

        /// <summary>
        ///     get the values of this value as string
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            string value;
            switch (TypeId) {
                case KnownTypeIds.ByteType:
                    value = data[0].ToString();
                    break;
                case KnownTypeIds.ShortInt:
                    value = ((sbyte)data[0]).ToString();
                    break;
                case KnownTypeIds.WordType:
                    value = BitConverter.ToUInt16(data, 0).ToString();
                    break;
                case KnownTypeIds.SmallInt:
                    value = BitConverter.ToInt16(data, 0).ToString();
                    break;
                case KnownTypeIds.CardinalType:
                    value = BitConverter.ToUInt32(data, 0).ToString();
                    break;
                case KnownTypeIds.IntegerType:
                    value = BitConverter.ToInt32(data, 0).ToString();
                    break;
                case KnownTypeIds.Uint64Type:
                    value = BitConverter.ToUInt64(data, 0).ToString();
                    break;
                case KnownTypeIds.Int64Type:
                    value = BitConverter.ToInt64(data, 0).ToString();
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return value;
        }

        /// <summary>
        ///     negate this value
        /// </summary>
        /// <returns></returns>
        public IValue Negate() {
            var result = new byte[9];

            if (!IsNegative && data[7] >= 0x80)
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);

            for (var i = 0; i < result.Length; i++) {
                result[i] = (byte)~data[i];
            }

            var carry = 0;
            for (var j = 0; j < result.Length; j++) {
                int sum;
                if (j == 0)
                    sum = result[j] + 1;
                else
                    sum = result[j] + carry;

                result[j] = (byte)(sum & 0xFF);
                carry = sum >> 8;
            }

            return new ScaledIntegerValue(result);
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj">other object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ScaledIntegerValue v) {
                return data.SequenceEqual(v.data);
            }

            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;

            for (var i = 0; i < data.Length; i++)
                result = result * 31 + data[i];

            return result;
        }

        /// <summary>
        ///     add another integer
        /// </summary>
        /// <param name="numberToAdd"></param>
        /// <returns></returns>
        public IValue Add(IValue numberToAdd) {
            if (numberToAdd is SpecialValue specialValue && (specialValue.Kind == SpecialConstantKind.InvalidInteger || specialValue.Kind == SpecialConstantKind.IntegerOverflow)) {
                return new SpecialValue(SpecialConstantKind.InvalidInteger);
            }

            var intValue = numberToAdd as ScaledIntegerValue;

            if (intValue == null)
                throw new ArgumentException();

            var result = new byte[9];
            var carry = 0;

            for (var j = 0; j < result.Length; j++) {
                var sum = data[j] + intValue.data[j] + carry;
                result[j] = (byte)(sum & 0xFF);
                carry = sum >> 8;
            }

            if ((result[8] & 1) != ((result[7] & 0x80) >> 7))
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);

            return new ScaledIntegerValue(result);
        }

        /// <summary>
        ///     subtract another integer
        /// </summary>
        /// <param name="numberToSubtract">number to subtract</param>
        /// <returns></returns>
        public IValue Subtract(IValue numberToSubtract) {
            if (numberToSubtract is SpecialValue specialValue && (specialValue.Kind == SpecialConstantKind.InvalidInteger || specialValue.Kind == SpecialConstantKind.IntegerOverflow)) {
                return new SpecialValue(SpecialConstantKind.InvalidInteger);
            }

            var intValue = numberToSubtract as ScaledIntegerValue;

            if (intValue == null)
                throw new ArgumentException();

            var negative = intValue.Negate();

            if (!(negative is ScaledIntegerValue))
                return negative;

            return Add(negative);
        }
    }
}
