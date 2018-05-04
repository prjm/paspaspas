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
        ///     type kind
        /// </summary>
        public abstract CommonTypeKind TypeKind { get; }

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

        internal static bool Equal(INumericalValue floatLeft, INumericalValue floatRight)
            => floatLeft == floatRight;

        internal static bool NotEqual(INumericalValue floatLeft, INumericalValue floatRight)
            => floatLeft != floatRight;

        internal static bool GreaterThenEqual(INumericalValue floatLeft, INumericalValue floatRight)
            => floatLeft.AsExtended >= floatRight.AsExtended;

        internal static bool GreaterThen(INumericalValue floatLeft, INumericalValue floatRight)
            => floatLeft.AsExtended > floatRight.AsExtended;

        internal static bool LessThenOrEqual(INumericalValue floatLeft, INumericalValue floatRight)
            => floatLeft.AsExtended <= floatRight.AsExtended;

        internal static bool LessThen(INumericalValue floatLeft, INumericalValue floatRight)
            => floatLeft.AsExtended < floatRight.AsExtended;

        /// <summary>
        ///     get an extended value
        /// </summary>
        /// <param name="number">number</param>
        /// <returns>number value</returns>
        public static IValue ToExtendedValue(in ExtF80 number)
            => new ExtendedValue(number);
    }
}
