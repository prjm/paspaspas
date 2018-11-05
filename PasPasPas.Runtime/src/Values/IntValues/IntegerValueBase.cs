using System;
using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values.IntValues {

    /// <summary>
    ///     base class for integer values
    /// </summary>
    public abstract class IntegerValueBase : IIntegerValue, IEquatable<IIntegerValue> {

        /// <summary>
        ///     get the type id
        /// </summary>
        public abstract int TypeId { get; }

        /// <summary>
        ///     check if the value is negative
        /// </summary>
        public abstract bool IsNegative { get; }

        /// <summary>
        ///     numerical value
        /// </summary>
        public abstract long SignedValue { get; }

        /// <summary>
        ///     numerical unsigned value
        /// </summary>
        public abstract ulong UnsignedValue { get; }

        /// <summary>
        ///     get the numerical value
        /// </summary>
        public abstract BigInteger AsBigInteger { get; }

        /// <summary>
        ///     get the value as extended value
        /// </summary>
        public ExtF80 AsExtended {
            get {
                if (TypeId == KnownTypeIds.Uint64Type)
                    return UnsignedValue;

                return SignedValue;
            }
        }

        /// <summary>
        ///     always <c>true</c> for constant integer values
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     type kind
        /// </summary>
        public abstract CommonTypeKind TypeKind { get; }

        /// <summary>
        ///     format this number as string
        /// </summary>
        /// <returns>number as string</returns>
        public abstract override string ToString();

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is IIntegerValue integer)
                return Equals(integer);

            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public abstract override int GetHashCode();

        /// <summary>
        ///     convert a big integer value to an integer value
        /// </summary>
        /// <param name="overflow">value used for overflow</param>
        /// <param name="value">value to convert</param>
        /// <returns>converted value</returns>
        public static ITypeReference ToIntValue(ITypeReference overflow, BigInteger value) {
            if (value < -9223372036854775808)
                return overflow;
            else if (value < -2147483648)
                return new Int64Value((long)value);
            else if (value < -32768)
                return new IntegerValue((int)value);
            else if (value < -128)
                return new SmallIntValue((short)value);
            else if (value < 128)
                return new ShortIntValue((sbyte)value);
            else if (value < 256)
                return new ByteValue((byte)value);
            else if (value < 32768)
                return new SmallIntValue((short)value);
            else if (value < 65536)
                return new WordValue((ushort)value);
            else if (value < 2147483648)
                return new IntegerValue((int)value);
            else if (value < 4294967296)
                return new CardinalValue((uint)value);
            else if (value < 9223372036854775808)
                return new Int64Value((long)value);
            else if (value <= 18446744073709551615)
                return new UInt64Value((ulong)value);

            return overflow;
        }

        internal static ITypeReference AddAndScale(ITypeReference overflow, IntegerValueBase intAugend, IntegerValueBase intAddend) {
            var s = intAugend.AsBigInteger + intAddend.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal static ITypeReference AndAndScale(ITypeReference overflow, IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            var s = firstOperator.AsBigInteger & secondOperator.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal static ITypeReference DivideAndScale(ITypeReference overflow, IntegerValueBase dividend, IntegerValueBase divisor) {
            var s = dividend.AsBigInteger / divisor.AsBigInteger;
            return ToIntValue(overflow, s);

        }


        internal static ITypeReference ModuloAndScale(ITypeReference overflow, IntegerValueBase dividend, IntegerValueBase divisor) {
            var s = dividend.AsBigInteger % divisor.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal static ITypeReference MultiplyAndScale(ITypeReference overflow, IntegerValueBase multiplicand, IntegerValueBase multiplier) {
            var s = multiplicand.AsBigInteger * multiplier.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal static ITypeReference Negate(ITypeReference overflow, IntegerValueBase intNumber) {
            var s = -intNumber.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal static ITypeReference Not(IntegerValueBase intNumber) => intNumber.InvertBits();

        internal static ITypeReference OrAndScale(ITypeReference overflow, IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            var s = firstOperator.AsBigInteger | secondOperator.AsBigInteger;
            return ToIntValue(overflow, s);

        }

        internal static ITypeReference ShlAndScale(ITypeReference overflow, IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            BigInteger s;
            if (firstOperator.TypeId == KnownTypeIds.Int64Type || firstOperator.TypeId == KnownTypeIds.Uint64Type)
                s = new BigInteger(firstOperator.SignedValue << (int)secondOperator.SignedValue);
            else
                s = new BigInteger((int)firstOperator.SignedValue << (int)secondOperator.SignedValue);

            return ToIntValue(overflow, s);
        }

        internal static ITypeReference ShrAndScale(ITypeReference overflow, IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            BigInteger s;
            if (firstOperator.TypeId == KnownTypeIds.Int64Type || firstOperator.TypeId == KnownTypeIds.Uint64Type)
                s = new BigInteger((ulong)firstOperator.SignedValue >> (int)secondOperator.SignedValue);
            else
                s = new BigInteger((uint)firstOperator.SignedValue >> (int)secondOperator.SignedValue);

            return ToIntValue(overflow, s);
        }

        internal static ITypeReference SubtractAndScale(ITypeReference overflow, IntegerValueBase intMinuend, IntegerValueBase intSubtrahend) {
            var s = intMinuend.AsBigInteger - intSubtrahend.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        internal static ITypeReference XorAndScale(ITypeReference overflow, IntegerValueBase firstInt, IntegerValueBase secondInt) {
            var s = firstInt.AsBigInteger ^ secondInt.AsBigInteger;
            return ToIntValue(overflow, s);
        }

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public static ITypeReference ToScaledIntegerValue(sbyte number)
            => new ShortIntValue(number);

        /// <summary>
        ///     convert a byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public static ITypeReference ToScaledIntegerValue(byte number) {
            if (number < 128)
                return new ShortIntValue((sbyte)number);

            return new ByteValue(number);
        }

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public static ITypeReference ToScaledIntegerValue(int number) {
            if (number < -32768)
                return new IntegerValue(number);
            else if (number < -128)
                return new SmallIntValue((short)number);
            else if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);
            else if (number < 32768)
                return new SmallIntValue((short)number);
            else if (number < 65536)
                return new WordValue((ushort)number);

            return new IntegerValue(number);
        }

        /// <summary>
        ///     convert a unsigned int a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public static ITypeReference ToScaledIntegerValue(uint number) {
            if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);
            else if (number < 32768)
                return new SmallIntValue((short)number);
            else if (number < 65536)
                return new WordValue((ushort)number);
            else if (number < 2147483648)
                return new IntegerValue((int)number);

            return new CardinalValue(number);
        }

        /// <summary>
        ///     convert a long to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public static ITypeReference ToScaledIntegerValue(long number) {
            if (number < -2147483648)
                return new Int64Value(number);
            else if (number < -32768)
                return new IntegerValue((int)number);
            else if (number < -128)
                return new SmallIntValue((short)number);
            else if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);
            else if (number < 32768)
                return new SmallIntValue((short)number);
            else if (number < 65536)
                return new WordValue((ushort)number);
            else if (number < 2147483648)
                return new IntegerValue((int)number);
            else if (number < 4294967296)
                return new CardinalValue((uint)number);

            return new Int64Value(number);
        }

        /// <summary>
        ///     convert a unsigned long to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public static ITypeReference ToScaledIntegerValue(ulong number) {
            if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);
            else if (number < 32768)
                return new SmallIntValue((short)number);
            else if (number < 65536)
                return new WordValue((ushort)number);
            else if (number < 2147483648)
                return new IntegerValue((int)number);
            else if (number < 4294967296)
                return new CardinalValue((uint)number);
            else if (number < 9223372036854775808)
                return new Int64Value((long)number);

            return new UInt64Value(number);
        }

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public static ITypeReference ToScaledIntegerValue(short number) {
            if (number < -128)
                return new SmallIntValue(number);
            else if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);

            return new SmallIntValue(number);
        }

        /// <summary>
        ///     convert a byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public static ITypeReference ToScaledIntegerValue(ushort number) {
            if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);
            else if (number < 32768)
                return new SmallIntValue((short)number);

            return new WordValue(number);
        }

        internal static bool GreaterThen(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger > secondInt.AsBigInteger;

        internal static bool GreaterThenEqual(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger >= secondInt.AsBigInteger;

        internal static bool Equal(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger == secondInt.AsBigInteger;

        internal static bool LessThenEqual(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger <= secondInt.AsBigInteger;

        internal static bool NotEqual(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger != secondInt.AsBigInteger;

        internal static bool LessThen(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => firstInt.AsBigInteger < secondInt.AsBigInteger;

        /// <summary>
        ///     invert all bits of this integer
        /// </summary>
        public abstract ITypeReference InvertBits();

        /// <summary>
        ///     increment value
        /// </summary>
        /// <param name="overflow"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ITypeReference Increment(ITypeReference overflow, IntegerValueBase value)
            => ToIntValue(overflow, value.AsBigInteger + BigInteger.One);

        /// <summary>
        ///     compare to another integer value
        /// </summary>
        /// <param name="other">other value</param>
        /// <returns></returns>
        public bool Equals(IIntegerValue other)
            => other.UnsignedValue == UnsignedValue;

        /// <summary>
        ///     absolute value
        /// </summary>
        /// <param name="overflow"></param>
        /// <param name="integerValue"></param>
        /// <returns></returns>
        public static ITypeReference AbsoluteValue(ITypeReference overflow, IntegerValueBase integerValue) {
            if (integerValue.IsNegative)
                return ToIntValue(overflow, BigInteger.Abs(integerValue.AsBigInteger));
            return integerValue;
        }
    }
}
