using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Runtime.Common;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     integer value with variable byte length
    /// </summary>
    public class ScaledIntegerValue : ValueBase, IIntegerValue {

        private byte[] data;

        private Bits CreateBits(bool isNegative) {
            var result = new Bits(72);
            if (isNegative)
                result.Invert();
            return result;
        }

        private byte[] CreateByteArray(bool isNegative, Bits bits) {
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
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">value of this integer</param>
        public ScaledIntegerValue(sbyte number) {
            IsNegative = number < 0;
            data = ByteArrayHelper.FromSignedByte(number);
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">value of this integer</param>
        public ScaledIntegerValue(byte number)
            => data = ByteArrayHelper.FromByte(number);

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">value of this integer</param>
        public ScaledIntegerValue(ushort number)
            => data = ByteArrayHelper.FromUnsignedShort(number);

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">value of this integer</param>
        public ScaledIntegerValue(short number) {
            IsNegative = number < 0;
            data = ByteArrayHelper.FromShort(number);
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">value of this integer</param>
        public ScaledIntegerValue(uint number)
            => data = ByteArrayHelper.FromUnsignedInteger(number);

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">value of this integer</param>
        public ScaledIntegerValue(int number) {
            IsNegative = number < 0;
            data = ByteArrayHelper.FromInteger(number);
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number">value of this integer</param>
        public ScaledIntegerValue(long number) {
            IsNegative = number < 0;
            data = ByteArrayHelper.FromLong(number);
        }

        /// <summary>
        ///     create a new integer value for a given number
        /// </summary>
        /// <param name="number"></param>
        public ScaledIntegerValue(ulong number) {
            data = ByteArrayHelper.FromUnsignedLong(number);
        }

        /// <summary>
        ///     create a new integer value for a given byte array
        /// </summary>
        /// <param name="bytes">byte-encoded integer value</param>
        /// <param name="isNegative"><c>true</c> if this is a negative number</param>
        public ScaledIntegerValue(bool isNegative, byte[] bytes) {
            IsNegative = isNegative;
            data = new byte[bytes.Length];
            Array.Copy(bytes, data, bytes.Length);
        }

        /// <summary>
        ///     get the data of this value
        /// </summary>
        public override byte[] Data {
            get {
                var result = new byte[data.Length];
                Array.Copy(data, result, result.Length);
                return result;
            }
        }

        /// <summary>
        ///     test if the number is negative
        /// </summary>
        public bool IsNegative { get; private set; }

        /// <summary>
        ///     get the matching type
        /// </summary>
        public override int TypeId {
            get {
                switch (data.Length) {
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

        /// <summary>
        ///     get the value as unsigned long
        /// </summary>
        public ulong AsUnsignedLong {
            get {
                var result = new byte[8];
                Array.Copy(data, result, data.Length);
                return BitConverter.ToUInt64(result, 0);
            }
        }

        /// <summary>
        ///     get the value as signed long
        /// </summary>
        public long AsSignedLong {
            get {
                var result = new byte[8];
                Array.Copy(data, result, data.Length);
                return BitConverter.ToInt64(result, 0);
            }
        }


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
            if (!IsNegative && data.Length > 7 && data[7] >= 0x80)
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);

            var result = CreateBits(IsNegative);
            var one = CreateBits(false);
            result.AsByteArray = data;
            result.Invert();
            one[0] = true;
            result.Add(one);
            return new ScaledIntegerValue(result.MostSignificantBit, CreateByteArray(result.MostSignificantBit, result));
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj">other object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ScaledIntegerValue v) {

                if (v.data.Length != data.Length)
                    return false;

                for (var i = 0; i < data.Length; i++)
                    if (data[i] != v.data[i])
                        return false;

                return true;
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

            var result = CreateBits(IsNegative);
            var otherValues = CreateBits(intValue.IsNegative);
            result.AsByteArray = data;
            otherValues.AsByteArray = intValue.data;
            result.Add(otherValues);

            if (result[64] != result[63])
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);

            return new ScaledIntegerValue(result.MostSignificantBit, CreateByteArray(result.MostSignificantBit, result));
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
