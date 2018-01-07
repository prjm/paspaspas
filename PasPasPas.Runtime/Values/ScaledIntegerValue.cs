using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     integer value with variable byte length
    /// </summary>
    public class ScaledIntegerValue : ValueBase, IIntegerValue {

        private byte[] data;

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
        public ScaledIntegerValue(ulong number)
            => data = ByteArrayHelper.FromUnsignedLong(number);

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
        ///     check if the number is negative
        /// </summary>
        public bool IsNegative { get; private set; }

        /// <summary>
        ///     get the matching type for this integer type
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
        public ulong AsUnsignedLong
            => ByteArrayHelper.ToUnsignedLong(data);

        /// <summary>
        ///     get the value as signed long
        /// </summary>
        public long AsSignedLong
            => ByteArrayHelper.ToSignedLong(data);

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

            var (isNegative, bytes) = ByteArrayHelper.TwoComplement(IsNegative, data);
            return new ScaledIntegerValue(isNegative, bytes);
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj">other object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ScaledIntegerValue v) {

                if (v.IsNegative != IsNegative)
                    return false;

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
        /// <param name="numberToAdd">add two integer</param>
        /// <returns></returns>
        public IValue Add(IValue numberToAdd) {
            if (numberToAdd is SpecialValue specialValue && (specialValue.Kind == SpecialConstantKind.InvalidInteger || specialValue.Kind == SpecialConstantKind.IntegerOverflow)) {
                return new SpecialValue(SpecialConstantKind.InvalidInteger);
            }

            var intValue = numberToAdd as ScaledIntegerValue;

            if (intValue == null)
                throw new ArgumentException();

            var (isNegative, bytes, overflow) = ByteArrayHelper.Add((IsNegative, data), (intValue.IsNegative, intValue.data));

            if (overflow)
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);

            return new ScaledIntegerValue(isNegative, bytes);
        }

        /// <summary>
        ///     subtract another integer
        /// </summary>
        /// <param name="numberToSubtract">number to subtract</param>
        /// <returns>subtraction results</returns>
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
