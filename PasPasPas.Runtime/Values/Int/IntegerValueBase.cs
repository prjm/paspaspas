﻿using System;
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
        public static IValue ToIntValue(ref BigInteger value) {
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
            return ToIntValue(ref s);
        }

        internal static IValue AndAndScale(IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            var s = firstOperator.AsBigInteger & secondOperator.AsBigInteger;
            return ToIntValue(ref s);
        }

        internal static IValue DivideAndScale(IntegerValueBase dividend, IntegerValueBase divisor) {
            var s = dividend.AsBigInteger / divisor.AsBigInteger;
            return ToIntValue(ref s);

        }


        internal static IValue ModuloAndScale(IntegerValueBase dividend, IntegerValueBase divisor) {
            var s = dividend.AsBigInteger % divisor.AsBigInteger;
            return ToIntValue(ref s);
        }

        internal static IValue MultiplyAndScale(IntegerValueBase multiplicand, IntegerValueBase multiplier) {
            var s = multiplicand.AsBigInteger * multiplier.AsBigInteger;
            return ToIntValue(ref s);
        }

        internal static IValue Negate(IntegerValueBase intNumber) {
            var s = -intNumber.AsBigInteger;
            return ToIntValue(ref s);
        }

        internal static IValue Not(IntegerValueBase intNumber) {
            var s = (-intNumber.AsBigInteger) - 1;
            return ToIntValue(ref s);
        }

        internal static IValue OrAndScale(IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            var s = firstOperator.AsBigInteger | secondOperator.AsBigInteger;
            return ToIntValue(ref s);

        }

        internal static IValue ShlAndScale(IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            BigInteger s;
            if (firstOperator.TypeId == KnownTypeIds.Int64Type || firstOperator.TypeId == KnownTypeIds.Uint64Type)
                s = new BigInteger(firstOperator.SignedValue << (int)secondOperator.SignedValue);
            else
                s = new BigInteger((int)firstOperator.SignedValue << (int)secondOperator.SignedValue);

            return ToIntValue(ref s);
        }

        internal static IValue ShrAndScale(IntegerValueBase firstOperator, IntegerValueBase secondOperator) {
            BigInteger s;
            if (firstOperator.TypeId == KnownTypeIds.Int64Type || firstOperator.TypeId == KnownTypeIds.Uint64Type)
                s = new BigInteger((ulong)firstOperator.SignedValue >> (int)secondOperator.SignedValue);
            else
                s = new BigInteger((uint)firstOperator.SignedValue >> (int)secondOperator.SignedValue);

            return ToIntValue(ref s);
        }

        internal static IValue SubtractAndScale(IntegerValueBase intMinuend, IntegerValueBase intSubtrahend) {
            var s = intMinuend.AsBigInteger - intSubtrahend.AsBigInteger;
            return ToIntValue(ref s);
        }

        internal static IValue XorAndScale(IntegerValueBase firstInt, IntegerValueBase secondInt) {
            var s = firstInt.AsBigInteger ^ secondInt.AsBigInteger;
            return ToIntValue(ref s);

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




    }
}