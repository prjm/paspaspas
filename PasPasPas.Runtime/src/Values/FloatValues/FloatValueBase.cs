using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values.FloatValues {

    /// <summary>
    ///     base class for float values
    /// </summary>
    public abstract class FloatValueBase : RuntimeValueBase, IRealNumberValue {

        private static int GetCommonTypeId(INumericalValue left, INumericalValue right = default) {

            if (left.TypeKind == CommonTypeKind.RealType)
                return left.TypeId;

            if (right != default && right.TypeKind == CommonTypeKind.RealType)
                return right.TypeId;

            return KnownTypeIds.Extended;
        }

        /// <summary>
        ///     generate a new float value
        /// </summary>
        /// <param name="typeId"></param>
        protected FloatValueBase(int typeId) : base(typeId) { }

        /// <summary>
        ///     test if the number is negative
        /// </summary>
        public abstract bool IsNegative { get; }

        /// <summary>
        ///     get float value
        /// </summary>
        public abstract ExtF80 AsExtended { get; }

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

        internal static ITypeReference Multiply(INumericalValue first, INumericalValue second)
            => new ExtendedValue(GetCommonTypeId(first, second), first.AsExtended * second.AsExtended);

        internal static ITypeReference Divide(INumericalValue numberDividend, INumericalValue numberDivisor)
            => new ExtendedValue(GetCommonTypeId(numberDividend, numberDivisor), numberDividend.AsExtended / numberDivisor.AsExtended);

        internal static ITypeReference Add(INumericalValue first, INumericalValue second)
            => new ExtendedValue(GetCommonTypeId(first, second), first.AsExtended + second.AsExtended);

        internal static ITypeReference Subtract(INumericalValue first, INumericalValue second)
            => new ExtendedValue(GetCommonTypeId(first, second), first.AsExtended - second.AsExtended);

        internal static ITypeReference Negate(INumericalValue value)
            => new ExtendedValue(GetCommonTypeId(value), -value.AsExtended);

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
        ///     absolute value
        /// </summary>
        /// <param name="floatValue"></param>
        /// <returns></returns>
        public static ITypeReference Abs(INumericalValue floatValue) {
            if (floatValue.IsNegative)
                return new ExtendedValue(GetCommonTypeId(floatValue), -floatValue.AsExtended);

            return floatValue;
        }

        /// <summary>
        ///     format value as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

    }
}
