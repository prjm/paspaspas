using System;
using System.Linq;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     helper for floating point numbers
    /// </summary>
    internal struct FloatingPointNumber {
        internal bool sign;
        internal byte[] significant;
        internal byte[] exponent;

        public FloatingPointNumber(FloatingPointValueKind kind) : this() {
            sign = false;

            switch (kind) {
                case FloatingPointValueKind.Single:
                    significant = new byte[3];
                    exponent = new byte[1];
                    break;

                case FloatingPointValueKind.Double:
                    significant = new byte[7];
                    exponent = new byte[2];
                    break;

                case FloatingPointValueKind.Extended:
                    significant = new byte[8];
                    exponent = new byte[2];
                    break;

                case FloatingPointValueKind.Real48:
                    significant = new byte[5];
                    exponent = new byte[1];
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }
    }

    /// <summary>
    ///     floating point value implementation
    /// </summary>
    public class FloatingPointValue : ValueBase {

        private byte[] data;

        /// <summary>
        ///     get the floating point data
        /// </summary>
        public override byte[] Data
            => data;

        /// <summary>
        ///     get the type id
        /// </summary>
        public override int TypeId {
            get {
                switch (GetKind(data.Length)) {
                    case FloatingPointValueKind.Single:
                        return KnownTypeIds.SingleType;
                    case FloatingPointValueKind.Double:
                        return KnownTypeIds.Double;
                    case FloatingPointValueKind.Extended:
                        return KnownTypeIds.Extended;
                    case FloatingPointValueKind.Real48:
                        return KnownTypeIds.Real48Type;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static int GetByteLength(FloatingPointValueKind kind) {
            switch (kind) {
                case FloatingPointValueKind.Single:
                    return 4;
                case FloatingPointValueKind.Double:
                    return 8;
                case FloatingPointValueKind.Extended:
                    return 10;
                case FloatingPointValueKind.Real48:
                    return 6;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }

        private static FloatingPointValueKind GetKind(int length) {
            switch (length) {
                case 4:
                    return FloatingPointValueKind.Single;
                case 8:
                    return FloatingPointValueKind.Double;
                case 10:
                    return FloatingPointValueKind.Extended;
                case 6:
                    return FloatingPointValueKind.Real48;
                default:
                    throw new ArgumentOutOfRangeException(nameof(length));

            }
        }

        private static FloatingPointNumber GetNumber(FloatingPointValueKind kind, double value) {
            var result = new FloatingPointNumber(kind);



            return result;
        }

        /// <summary>
        ///     float as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => "X";

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is FloatingPointValue f) {
                return f.data.SequenceEqual(data);
            }

            return false;
        }

        /// <summary>
        ///     get the hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;

            for (var i = 0; i < data.Length; i++)
                result = result * 31 + data[i];

            return result;
        }
        /// <summary>
        ///     create a new floating point value
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="value"></param>
        public FloatingPointValue(FloatingPointValueKind kind, double value)
            : this(kind, GetNumber(kind, value)) { }

        /// <summary>
        ///     create a new floating point value
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="number"></param>
        internal FloatingPointValue(FloatingPointValueKind kind, in FloatingPointNumber number) {
            data = new byte[GetByteLength(kind)];

            if (number.sign)
                data[data.Length - 1] = 0x80;

            switch (kind) {
                case FloatingPointValueKind.Single:
                    data[0] = number.significant[0];
                    data[1] = number.significant[1];
                    data[2] = (byte)((number.significant[2] | (number.exponent[0] << 7)) & 0xFF);
                    data[3] = (byte)((number.exponent[0] >> 1) & 0xFF);
                    if (number.sign)
                        data[3] = (byte)((data[5] | 0x80) & 0xFF);
                    break;

                case FloatingPointValueKind.Double:
                    data[0] = number.significant[0];
                    data[1] = number.significant[1];
                    data[2] = number.significant[2];
                    data[3] = number.significant[3];
                    data[4] = number.significant[4];
                    data[5] = number.significant[5];
                    data[6] = (byte)(((number.significant[6] & 0xFF) | ((number.exponent[0] << 4) & 0xFF)) & 0xFF);
                    data[7] = (byte)((((number.exponent[0] >> 4) & 0xFF) | number.exponent[1]) & 0xFF);
                    if (number.sign)
                        data[7] = (byte)((data[7] | 0x80) & 0xFF);
                    break;

                case FloatingPointValueKind.Extended:
                    data[0] = number.significant[0];
                    data[1] = number.significant[1];
                    data[2] = number.significant[2];
                    data[3] = number.significant[3];
                    data[4] = number.significant[4];
                    data[5] = number.significant[5];
                    data[6] = number.significant[6];
                    data[7] = (byte)(number.significant[7] & 0x7f);
                    data[8] = number.exponent[0];
                    data[9] = (byte)(number.exponent[1] & 0x7F);
                    if (number.sign)
                        data[9] = (byte)((data[9] | 0x80) & 0xFF);
                    break;

                case FloatingPointValueKind.Real48:
                    data[0] = number.exponent[0];
                    data[1] = number.significant[0];
                    data[2] = number.significant[1];
                    data[3] = number.significant[2];
                    data[4] = number.significant[3];
                    data[5] = number.significant[4];
                    if (number.sign)
                        data[5] = (byte)((data[5] | 0x80) & 0xFF);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }
    }
}