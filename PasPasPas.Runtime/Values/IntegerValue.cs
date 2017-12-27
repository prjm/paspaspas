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
