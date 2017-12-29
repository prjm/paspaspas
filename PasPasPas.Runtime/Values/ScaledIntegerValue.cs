using System;
using System.Linq;
using PasPasPas.Infrastructure.Common;
using PasPasPas.Typings.Common;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     integer value
    /// </summary>
    public class ScaledIntegerValue : ValueBase, IIntegerValue {

        private byte[] data = new byte[9];

        private static void Fill(byte[] data, byte value) {
            for (var i = 0; i < data.Length; i++)
                data[i] = value;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(sbyte number) {
            if (number < 0)
                Fill(data, 0xFF);

            data[0] = (byte)number;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(byte number)
            => data[0] = number;

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(short number) {
            if (number < 0)
                Fill(data, 0xFF);

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
                Fill(data, 0xFF);

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
                Fill(data, 0xFF);

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
                        return TypeIds.ShortInt;
                    case 1:
                        return IsNegative || data[0] < 0x80 ? TypeIds.ShortInt : TypeIds.ByteType;
                    case 2:
                        return IsNegative || data[1] < 0x80 ? TypeIds.SmallInt : TypeIds.WordType;
                    case 3:
                    case 4:
                        return IsNegative || data[3] < 0x80 ? TypeIds.IntegerType : TypeIds.CardinalType;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        return IsNegative || data[7] < 0x80 ? TypeIds.Int64Type : TypeIds.Uint64Type;
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
                case TypeIds.ByteType:
                    value = data[0].ToString();
                    break;
                case TypeIds.ShortInt:
                    value = ((sbyte)data[0]).ToString();
                    break;
                case TypeIds.WordType:
                    value = BitConverter.ToUInt16(data, 0).ToString();
                    break;
                case TypeIds.SmallInt:
                    value = BitConverter.ToInt16(data, 0).ToString();
                    break;
                case TypeIds.CardinalType:
                    value = BitConverter.ToUInt32(data, 0).ToString();
                    break;
                case TypeIds.IntegerType:
                    value = BitConverter.ToInt32(data, 0).ToString();
                    break;
                case TypeIds.Uint64Type:
                    value = BitConverter.ToUInt64(data, 0).ToString();
                    break;
                case TypeIds.Int64Type:
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
        ///     check for equalisty
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ScaledIntegerValue v) {
                return data.SequenceEqual(v.data);
            }

            return false;
        }

        /// <summary>
        ///     hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;

            for (var i = 0; i < data.Length; i++)
                result = result * 31 + data[i];

            return result;
        }

    }
}
