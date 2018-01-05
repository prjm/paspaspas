using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Runtime.Common;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     integer value with variable byte length
    /// </summary>
    /// <remarks>internally implemented as 9-byte integer values</remarks>
    public class ScaledIntegerValue : ValueBase, IIntegerValue {

        private readonly Bits data = new Bits(72);

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">number value</param>
        public ScaledIntegerValue(sbyte number) {
            if (number < 0)
                data.Invert();
            data.LeastSignificantSignedByte = number;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">number value</param>
        public ScaledIntegerValue(short number) {
            if (number < 0)
                data.Invert();
            data.LeastSignificantSignedWord = number;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(int number) {
            if (number < 0)
                data.Invert();
            data.LeastSignificantSignedDoubleWord = number;
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(long number) {
            if (number < 0)
                data.Invert();
            data.LeastSignificantSignedQuadWord = number;
        }


        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">number value</param>
        public ScaledIntegerValue(byte number)
            => data.LeastSignificantByte = number;

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(ushort number)
            => data.LeastSignificantWord = number;

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(uint number)
            => data.LeastSignificantDoubleWord = number;

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(ulong number) {
            data.LeastSignificantQuadWord = number;
        }

        /// <summary>
        ///     create a new integer value for a given byte array
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(Bits number) {
            if (number.Length != data.Length)
                throw new ArgumentOutOfRangeException(nameof(number));
            data.Assign(number);
        }

        /// <summary>
        ///     get the data of this value
        /// </summary>
        public override byte[] Data {
            get {
                var len = InternalLength;
                var internalData = data.AsByteArray;
                var result = new byte[len];
                Array.Copy(internalData, result, result.Length);
                return result;
            }
        }

        /// <summary>
        ///     test if the number is negative
        /// </summary>
        public bool IsNegative
            => data[71];

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
                        return IsNegative || !data[7] ? KnownTypeIds.ShortInt : KnownTypeIds.ByteType;
                    case 2:
                        return IsNegative || !data[15] ? KnownTypeIds.SmallInt : KnownTypeIds.WordType;
                    case 4:
                        return IsNegative || !data[31] ? KnownTypeIds.IntegerType : KnownTypeIds.CardinalType;
                    case 8:
                        return IsNegative || !data[63] ? KnownTypeIds.Int64Type : KnownTypeIds.Uint64Type;
                }
                throw new InvalidOperationException();
            }
        }

        private int InternalLength {
            get {
                int length;
                var bytes = data.AsByteArray;

                for (length = bytes.Length - 1; length > 1; length--) {
                    if (IsNegative && bytes[length - 1] != 0xFF)
                        break;
                    if (!IsNegative && bytes[length - 1] != 0x00)
                        break;
                }

                if (IsNegative && bytes[length - 1] < 0x80)
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
            => data.LeastSignificantQuadWord;

        /// <summary>
        ///     get the values of this value as string
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            string value;
            var array = Data;
            switch (TypeId) {
                case KnownTypeIds.ByteType:
                    value = array[0].ToString();
                    break;
                case KnownTypeIds.ShortInt:
                    value = unchecked((sbyte)array[0]).ToString();
                    break;
                case KnownTypeIds.WordType:
                    value = BitConverter.ToUInt16(array, 0).ToString();
                    break;
                case KnownTypeIds.SmallInt:
                    value = BitConverter.ToInt16(array, 0).ToString();
                    break;
                case KnownTypeIds.CardinalType:
                    value = BitConverter.ToUInt32(array, 0).ToString();
                    break;
                case KnownTypeIds.IntegerType:
                    value = BitConverter.ToInt32(array, 0).ToString();
                    break;
                case KnownTypeIds.Uint64Type:
                    value = BitConverter.ToUInt64(array, 0).ToString();
                    break;
                case KnownTypeIds.Int64Type:
                    value = BitConverter.ToInt64(array, 0).ToString();
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
            if (!IsNegative && data[63])
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);

            var result = new Bits(data);
            var one = new Bits(data.Length);
            result.Invert();
            one[0] = true;
            result.Add(one);
            return new ScaledIntegerValue(result);
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj">other object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ScaledIntegerValue v) {
                return data.Equals(v.data);
            }

            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => data.GetHashCode();


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

            var result = new Bits(data);
            result.Add(intValue.data);

            if (result[64] != result[63])
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
