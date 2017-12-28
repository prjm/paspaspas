using System;
using PasPasPas.Infrastructure.Common;
using PasPasPas.Typings.Common;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     integer value
    /// </summary>
    public class IntegerValue : ValueBase {

        private byte[] data;
        private readonly bool signed;

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public IntegerValue(sbyte number) {
            data = new byte[] { (byte)number };
            signed = true;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public IntegerValue(byte number) {
            data = new byte[] { number };
            signed = false;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public IntegerValue(short number) {
            data = BitConverter.GetBytes(number);
            signed = true;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public IntegerValue(ushort number) {
            data = BitConverter.GetBytes(number);
            signed = false;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public IntegerValue(int number) {
            data = BitConverter.GetBytes(number);
            signed = true;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public IntegerValue(uint number) {
            data = BitConverter.GetBytes(number);
            signed = false;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public IntegerValue(long number) {
            data = BitConverter.GetBytes(number);
            signed = true;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public IntegerValue(ulong number) {
            data = BitConverter.GetBytes(number);
            signed = false;
        }

        /// <summary>
        ///     create a new integer value for a given byte array
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isSigned"></param>
        public IntegerValue(byte[] number, bool isSigned) {
            data = number;
            signed = isSigned;
        }

        /// <summary>
        ///     get the data of this value
        /// </summary>
        public override byte[] Data
            => data;

        /// <summary>
        ///     get the matching type
        /// </summary>
        public override int TypeId {
            get {
                switch (data.Length) {
                    case 1:
                        return signed ? TypeIds.ShortInt : TypeIds.ByteType;
                    case 2:
                        return signed ? TypeIds.SmallInt : TypeIds.WordType;
                    case 4:
                        return signed ? TypeIds.IntegerType : TypeIds.CardinalType;
                    case 8:
                        return signed ? TypeIds.Int64Type : TypeIds.Uint64Type;
                }
                throw new InvalidOperationException();
            }
        }

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
        public IntegerValue Negate() {
            var currentlySigned = signed;
            var length = data.Length;
            var extend = 0;

            if (signed && length == 1 && data[0] == 0x80)
                currentlySigned = false;
            else if (signed && length == 2 && data[0] == 0x00 && data[1] == 0x80)
                currentlySigned = false;
            else if (signed && length == 4 && data[0] == 0x00 && data[1] == 0x00 && data[2] == 0x00 && data[3] == 0x80)
                currentlySigned = false;
            else if (signed && length == 8 && data[0] == 0xFF && data[1] == 0xFF && data[2] == 0xFF && data[3] == 0xFF && data[4] == 0xFF && data[5] == 0xFF && data[6] == 0xFF && data[7] == 0x80)
                currentlySigned = false;

            if (signed && length == 2 && data[1] == 0xFF && data[0] <= 0x80) {
                extend = -1;
                currentlySigned = false;
            }
            else if (signed && length == 4 && data[3] == 0xFF && data[2] == 0xFF && data[1] <= 0x80) {
                extend = -2;
                currentlySigned = false;
            }
            else if (signed && length == 8 && data[7] == 0xFF && data[6] == 0xFF && data[5] == 0xFF && data[4] == 0xFF && data[3] <= 0x80) {
                extend = -4;
                currentlySigned = false;
            }

            if (!signed)
                currentlySigned = true;

            if (!signed && length == 1 && data[0] > 0x80)
                extend = 1;
            else if (!signed && length == 2 && data[0] == 0xFF && data[1] > 0x80)
                extend = 2;
            else if (!signed && length == 4 && data[0] == 0xFF && data[1] == 0xFF && data[2] == 0xFF && data[3] > 0x80)
                extend = 4;

            var result = new byte[data.Length + extend];

            for (var i = 0; i < result.Length; i++) {
                if (i >= data.Length)
                    result[i] = 0xFF;
                else
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

            return new IntegerValue(result, currentlySigned);
        }

        /*
                private byte[] Encode(ulobjong result) {
                    if (result < 128)
                        return BitConverter.GetBytes((sbyte)result);
                    else if (result < 256)
                        return BitConverter.GetBytes((byte)result);
                    else if (result < 32768)
                        return BitConverter.GetBytes((short)result);
                    else if (result < 65536)
                        return BitConverter.GetBytes((ushort)result);
                    else if (result < 2147483648)
                        return BitConverter.GetBytes((int)result);
                    else if (result < 4294967296)
                        return BitConverter.GetBytes((uint)result);
                    else if (result < 9223372036854775808)
                        return BitConverter.GetBytes((long)result);
                    else
                        return BitConverter.GetBytes(result);
                }

          */
    }
}
