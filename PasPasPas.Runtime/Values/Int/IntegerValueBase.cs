using System.Numerics;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Runtime.Values.Boolean;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values.Int {

    /// <summary>
    ///     base class for integer values
    /// </summary>
    public abstract class IntegerValueBase : IIntegerValue {

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
        ///     format this number as string
        /// </summary>
        /// <returns>number as string</returns>
        public abstract override string ToString();

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract override bool Equals(object obj);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public abstract override int GetHashCode();

        /// <summary>
        ///     convert a big integer value to a int value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IValue ToIntValue(in BigInteger value) {
            if (value < -9223372036854775808)
                return new SpecialValue(SpecialConstantKind.IntegerOverflow);
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

            return new SpecialValue(SpecialConstantKind.IntegerOverflow);
        }

        internal static IValue AddAndScale(IntegerValueBase intAugend, IntegerValueBase intAddend) {
            var s = intAugend.AsBigInteger + intAddend.AsBigInteger;
            return ToIntValue(s);
        }

        internal static IValue AndAndScale(IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            var s = firstOperator.AsBigInteger & secondOperator.AsBigInteger;
            return ToIntValue(s);
        }

        internal static IValue DivideAndScale(IntegerValueBase dividend, IntegerValueBase divisor) {
            var s = dividend.AsBigInteger / divisor.AsBigInteger;
            return ToIntValue(s);

        }


        internal static IValue ModuloAndScale(IntegerValueBase dividend, IntegerValueBase divisor) {
            var s = dividend.AsBigInteger % divisor.AsBigInteger;
            return ToIntValue(s);
        }

        internal static IValue MultiplyAndScale(IntegerValueBase multiplicand, IntegerValueBase multiplier) {
            var s = multiplicand.AsBigInteger * multiplier.AsBigInteger;
            return ToIntValue(s);
        }

        internal static IValue Negate(IntegerValueBase intNumber) {
            var s = -intNumber.AsBigInteger;
            return ToIntValue(s);
        }

        internal static IValue Not(IntegerValueBase intNumber) {
            return intNumber.InvertBits();
        }

        internal static IValue OrAndScale(IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            var s = firstOperator.AsBigInteger | secondOperator.AsBigInteger;
            return ToIntValue(s);

        }

        internal static IValue ShlAndScale(IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            BigInteger s;
            if (firstOperator.TypeId == KnownTypeIds.Int64Type || firstOperator.TypeId == KnownTypeIds.Uint64Type)
                s = new BigInteger(firstOperator.SignedValue << (int)secondOperator.SignedValue);
            else
                s = new BigInteger((int)firstOperator.SignedValue << (int)secondOperator.SignedValue);

            return ToIntValue(s);
        }

        internal static IValue ShrAndScale(IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            BigInteger s;
            if (firstOperator.TypeId == KnownTypeIds.Int64Type || firstOperator.TypeId == KnownTypeIds.Uint64Type)
                s = new BigInteger((ulong)firstOperator.SignedValue >> (int)secondOperator.SignedValue);
            else
                s = new BigInteger((uint)firstOperator.SignedValue >> (int)secondOperator.SignedValue);

            return ToIntValue(s);
        }

        internal static IValue SubtractAndScale(IntegerValueBase intMinuend, IntegerValueBase intSubtrahend) {
            var s = intMinuend.AsBigInteger - intSubtrahend.AsBigInteger;
            return ToIntValue(s);
        }

        internal static IValue XorAndScale(IntegerValueBase firstInt, IntegerValueBase secondInt) {
            var s = firstInt.AsBigInteger ^ secondInt.AsBigInteger;
            return ToIntValue(s);
        }

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public static IValue ToScaledIntegerValue(sbyte number)
            => new ShortIntValue(number);

        /// <summary>
        ///     convert a byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public static IValue ToScaledIntegerValue(byte number) {
            if (number < 128)
                return new ShortIntValue((sbyte)number);

            return new ByteValue(number);
        }

        /// <summary>
        ///     convert a signed byte to a constant value
        /// </summary>
        /// <param name="number">numerical value</param>
        /// <returns></returns>
        public static IValue ToScaledIntegerValue(int number) {
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
        public static IValue ToScaledIntegerValue(uint number) {
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
        public static IValue ToScaledIntegerValue(long number) {
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
        public static IValue ToScaledIntegerValue(ulong number) {
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
        public static IValue ToScaledIntegerValue(short number) {
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
        public static IValue ToScaledIntegerValue(ushort number) {
            if (number < 128)
                return new ShortIntValue((sbyte)number);
            else if (number < 256)
                return new ByteValue((byte)number);
            else if (number < 32768)
                return new SmallIntValue((short)number);

            return new WordValue(number);
        }

        internal static IValue GreaterThen(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => new BooleanValue(firstInt.AsBigInteger > secondInt.AsBigInteger);

        internal static IValue GreaterThenEqual(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => new BooleanValue(firstInt.AsBigInteger >= secondInt.AsBigInteger);

        internal static IValue Equal(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => new BooleanValue(firstInt.AsBigInteger == secondInt.AsBigInteger);

        internal static IValue LessThenEqual(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => new BooleanValue(firstInt.AsBigInteger <= secondInt.AsBigInteger);

        internal static IValue NotEqual(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => new BooleanValue(firstInt.AsBigInteger != secondInt.AsBigInteger);

        internal static IValue LessThen(IntegerValueBase firstInt, IntegerValueBase secondInt)
            => new BooleanValue(firstInt.AsBigInteger < secondInt.AsBigInteger);

        /// <summary>
        ///     invert all bits of this integer
        /// </summary>
        public abstract IValue InvertBits();
    }
}
