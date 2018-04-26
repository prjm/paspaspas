using System;
using PasPasPas.Global.Runtime;
using PasPasPas.Runtime.Values.Boolean;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values.Float {

    /// <summary>
    ///     base class for float values
    /// </summary>
    public abstract class FloatValueBase : IRealNumberValue {

        /// <summary>
        ///     test if the number is negative
        /// </summary>
        public abstract bool IsNegative { get; }

        /// <summary>
        ///     get float value
        /// </summary>
        public abstract ExtF80 AsExtended { get; }

        /// <summary>
        ///     type id
        /// </summary>
        public abstract int TypeId { get; }

        /// <summary>
        ///     always <c>true</c> for floating-point numbers
        /// </summary>
        public bool IsConstant
            => true;

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
        ///     format this number as string
        /// </summary>
        /// <returns>number as string</returns>
        public abstract override string ToString();

        internal static IValue Multiply(INumericalValue first, INumericalValue second)
            => new ExtendedValue(first.AsExtended * second.AsExtended);

        internal static IValue Divide(INumericalValue numberDividend, INumericalValue numberDivisor)
            => new ExtendedValue(numberDividend.AsExtended / numberDivisor.AsExtended);

        internal static IValue Add(INumericalValue first, INumericalValue second)
            => new ExtendedValue(first.AsExtended + second.AsExtended);

        internal static IValue Subtract(INumericalValue first, INumericalValue second)
            => new ExtendedValue(first.AsExtended - second.AsExtended);

        internal static IValue Negate(INumericalValue value)
            => new ExtendedValue(-value.AsExtended);

        internal static IValue Equal(INumericalValue floatLeft, INumericalValue floatRight)
            => new BooleanValue(floatLeft == floatRight);

        internal static IValue NotEqual(INumericalValue floatLeft, INumericalValue floatRight)
            => new BooleanValue(floatLeft != floatRight);

        internal static IValue GreaterThenEqual(INumericalValue floatLeft, INumericalValue floatRight)
            => new BooleanValue(floatLeft.AsExtended >= floatRight.AsExtended);

        internal static IValue GreaterThen(INumericalValue floatLeft, INumericalValue floatRight)
            => new BooleanValue(floatLeft.AsExtended > floatRight.AsExtended);

        internal static IValue LessThenOrEqual(INumericalValue floatLeft, INumericalValue floatRight)
            => new BooleanValue(floatLeft.AsExtended <= floatRight.AsExtended);

        internal static IValue LessThen(INumericalValue floatLeft, INumericalValue floatRight)
            => new BooleanValue(floatLeft.AsExtended < floatRight.AsExtended);
    }
}
