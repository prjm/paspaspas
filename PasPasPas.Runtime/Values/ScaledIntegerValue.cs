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
        ///     create a new integer value for a given byte array calculation
        /// </summary>
        public ScaledIntegerValue(ByteArrayCalculation result) {
            IsNegative = result.IsNegative;
            data = result.Data;
        }

        /// <summary>
        ///     get the binary data of this value
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

            switch (TypeId) {
                case KnownTypeIds.ByteType:
                    return data[0].ToString();

                case KnownTypeIds.ShortInt:
                    return ((sbyte)data[0]).ToString();

                case KnownTypeIds.WordType:
                    return BitConverter.ToUInt16(data, 0).ToString();

                case KnownTypeIds.SmallInt:
                    return BitConverter.ToInt16(data, 0).ToString();

                case KnownTypeIds.CardinalType:
                    return BitConverter.ToUInt32(data, 0).ToString();

                case KnownTypeIds.IntegerType:
                    return BitConverter.ToInt32(data, 0).ToString();

                case KnownTypeIds.Uint64Type:
                    return BitConverter.ToUInt64(data, 0).ToString();

                case KnownTypeIds.Int64Type:
                    return BitConverter.ToInt64(data, 0).ToString();

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     negate this value
        /// </summary>
        /// <returns>two complement of this value</returns>
        public IValue Negate() {
            if (!IsNegative && data.Length > 7 && data[7] >= 0x80)
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);

            var result = ByteArrayHelper.TwoComplement(8, IsNegative, data);
            return new ScaledIntegerValue(result);
        }

        private ScaledIntegerValue AsScaledInteger(IValue value) {
            if (value is SpecialValue specialValue && (specialValue.Kind == SpecialConstantKind.InvalidInteger || specialValue.Kind == SpecialConstantKind.IntegerOverflow)) {
                return null;
            }

            var intValue = value as ScaledIntegerValue;

            if (intValue == null)
                throw new ArgumentException();

            return intValue;
        }

        /// <summary>
        ///     add another integer to this integer value
        /// </summary>
        /// <param name="numberToAdd">add two integer</param>
        /// <returns></returns>
        public IValue Add(IValue numberToAdd) {
            var addend = AsScaledInteger(numberToAdd);

            if (addend == null)
                return new SpecialValue(SpecialConstantKind.InvalidInteger);

            var left = new ByteArrayCalculation(IsNegative, Data);
            var right = new ByteArrayCalculation(addend.IsNegative, addend.Data);
            var result = ByteArrayHelper.Add(8, left, right);

            if (result.Overflow)
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);

            return new ScaledIntegerValue(result.IsNegative, result.Data);
        }

        /// <summary>
        ///     subtract another integer
        /// </summary>
        /// <param name="numberToSubtract">number to subtract</param>
        /// <returns>subtraction results</returns>
        public IValue Subtract(IValue numberToSubtract) {
            var subtrahend = AsScaledInteger(numberToSubtract);
            if (subtrahend == null)
                return new SpecialValue(SpecialConstantKind.InvalidInteger);

            var negative = subtrahend.Negate();

            if (!(negative is ScaledIntegerValue))
                return negative;

            return Add(negative);
        }

        /// <summary>
        ///     multiply with another integer value
        /// </summary>
        /// <param name="numberToMultiply">number to multiply with</param>
        /// <returns>subtraction results</returns>
        public IValue Multiply(IValue numberToMultiply) {
            var multiplier = AsScaledInteger(numberToMultiply);
            if (multiplier == null) {
                return new SpecialValue(SpecialConstantKind.InvalidInteger);
            }

            var left = new ByteArrayCalculation(IsNegative, Data);
            var right = new ByteArrayCalculation(multiplier.IsNegative, multiplier.Data);

            var result = ByteArrayHelper.Multiply(8, left, right);

            if (result.Overflow)
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);

            return new ScaledIntegerValue(result.IsNegative, result.Data);
        }

        /// <summary>
        ///     divide this number by another number
        /// </summary>
        /// <param name="numberToDivide"></param>
        /// <returns></returns>
        public IValue Divide(IValue numberToDivide) {
            var divisor = AsScaledInteger(numberToDivide);
            if (divisor == null) {
                return new SpecialValue(SpecialConstantKind.InvalidInteger);
            }

            if (divisor.AsSignedLong == 0) {
                return new SpecialValue(SpecialConstantKind.DivisionByZero);
            }

            var left = new ByteArrayCalculation(IsNegative, Data);
            var right = new ByteArrayCalculation(divisor.IsNegative, divisor.Data);

            var result = ByteArrayHelper.Divide(8, left, right);

            if (result.Overflow)
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);

            return new ScaledIntegerValue(result.IsNegative, result.Data);
        }

        /// <summary>
        ///     divide this number by another number and get the remainder
        /// </summary>
        /// <param name="numberToDivide"></param>
        /// <returns></returns>
        public IValue Modulo(IValue numberToDivide) {
            var divisor = AsScaledInteger(numberToDivide);
            if (divisor == null) {
                return new SpecialValue(SpecialConstantKind.InvalidInteger);
            }

            if (divisor.AsSignedLong == 0) {
                return new SpecialValue(SpecialConstantKind.DivisionByZero);
            }

            var left = new ByteArrayCalculation(IsNegative, Data);
            var right = new ByteArrayCalculation(divisor.IsNegative, divisor.Data);

            var result = ByteArrayHelper.Modulo(8, left, right);

            if (result.Overflow)
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);

            return new ScaledIntegerValue(result.IsNegative, result.Data);
        }

        /// <summary>
        ///     invert all bits of this integer
        /// </summary>
        /// <returns></returns>
        public IValue Not() {
            var operand = new ByteArrayCalculation(IsNegative, Data);
            var result = ByteArrayHelper.Not(8, operand);
            return new ScaledIntegerValue(result.IsNegative, result.Data);
        }

        /// <summary>
        ///     bitwise and
        /// </summary>
        /// <param name="valueToAnd"></param>
        /// <returns></returns>
        public IValue And(IValue valueToAnd) {
            var operand = AsScaledInteger(valueToAnd);
            if (operand == null) {
                return new SpecialValue(SpecialConstantKind.InvalidInteger);
            }

            var left = new ByteArrayCalculation(IsNegative, Data);
            var right = new ByteArrayCalculation(operand.IsNegative, operand.Data);
            var result = ByteArrayHelper.And(8, left, right);
            return new ScaledIntegerValue(result.IsNegative, result.Data);
        }

        /// <summary>
        ///     bitwise or
        /// </summary>
        /// <param name="valueToOr">operand</param>
        /// <returns></returns>
        public IValue Or(IValue valueToOr) {
            var operand = AsScaledInteger(valueToOr);
            if (operand == null) {
                return new SpecialValue(SpecialConstantKind.InvalidInteger);
            }

            var left = new ByteArrayCalculation(IsNegative, Data);
            var right = new ByteArrayCalculation(operand.IsNegative, operand.Data);
            var result = ByteArrayHelper.Or(8, left, right);
            return new ScaledIntegerValue(result.IsNegative, result.Data);
        }

        /// <summary>
        ///     bitwise xor
        /// </summary>
        /// <param name="valueToXor">operand</param>
        /// <returns></returns>
        public IValue Xor(IValue valueToXor) {
            var operand = AsScaledInteger(valueToXor);
            if (operand == null) {
                return new SpecialValue(SpecialConstantKind.InvalidInteger);
            }

            var left = new ByteArrayCalculation(IsNegative, Data);
            var right = new ByteArrayCalculation(operand.IsNegative, operand.Data);
            var result = ByteArrayHelper.Xor(8, left, right);
            return new ScaledIntegerValue(result.IsNegative, result.Data);
        }

        /// <summary>
        ///     shift left
        /// </summary>
        /// <param name="numberOfBits">operand</param>
        /// <returns></returns>
        public IValue Shl(IValue numberOfBits) {
            var operand = AsScaledInteger(numberOfBits);
            if (operand == null) {
                return new SpecialValue(SpecialConstantKind.InvalidInteger);
            }

            var left = new ByteArrayCalculation(IsNegative, Data);
            var right = new ByteArrayCalculation(operand.IsNegative, operand.Data);
            var typeId = TypeId;
            var numberOfBytes = (typeId == KnownTypeIds.Int64Type || typeId == KnownTypeIds.Uint64Type) ? 8 : 4;
            var result = ByteArrayHelper.Shl(numberOfBytes, left, right);
            return new ScaledIntegerValue(result.IsNegative, result.Data);
        }

        /// <summary>
        ///     shift right
        /// </summary>
        /// <param name="numberOfBits">operand</param>
        /// <returns></returns>
        public IValue Shr(IValue numberOfBits) {
            var operand = AsScaledInteger(numberOfBits);
            if (operand == null) {
                return new SpecialValue(SpecialConstantKind.InvalidInteger);
            }

            var left = new ByteArrayCalculation(IsNegative, Data);
            var right = new ByteArrayCalculation(operand.IsNegative, operand.Data);
            var typeId = TypeId;
            var numberOfBytes = (typeId == KnownTypeIds.Int64Type || typeId == KnownTypeIds.Uint64Type) ? 8 : 4;
            var result = ByteArrayHelper.Shr(numberOfBytes, left, right);
            return new ScaledIntegerValue(result.IsNegative, result.Data);
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
        public override int GetHashCode() {
            unchecked {
                var result = 17;

                if (IsNegative)
                    result = result * 31 + 17;

                for (var i = 0; i < data.Length; i++)
                    result = result * 31 + data[i];

                return result;
            }
        }

    }
}
