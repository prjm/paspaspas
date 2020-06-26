using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values.FloatValues {

    /// <summary>
    ///     base class for float values
    /// </summary>
    internal abstract class FloatValueBase : RuntimeValueBase, IRealNumberValue {

        /// <summary>
        ///     generate a new float value
        /// </summary>
        /// <param name="typeDef"></param>
        /// <param name="kind"></param>
        protected FloatValueBase(ITypeDefinition typeDef, RealTypeKind kind) : base(typeDef) {
            if (typeDef.BaseType != BaseType.Real)
                throw new ArgumentException(string.Empty, nameof(typeDef));

            if (!(typeDef is IRealType realType))
                throw new ArgumentException(string.Empty, nameof(typeDef));

            if (realType.Kind != kind)
                throw new ArgumentException(string.Empty, nameof(typeDef));
        }

        /// <summary>
        ///     test if the number is negative
        /// </summary>
        public abstract bool IsNegative { get; }

        /// <summary>
        ///     get float value
        /// </summary>
        public abstract ExtF80 AsExtended { get; }

        /// <summary>
        ///     real type
        /// </summary>
        public IRealType RealType
            => TypeDefinition as IRealType ?? throw new InvalidOperationException();

        internal static IValue Multiply(ITypeDefinition typeDef, INumericalValue first, INumericalValue second)
            => new ExtendedValue(typeDef, first.AsExtended * second.AsExtended);

        internal static IValue Divide(ITypeDefinition typeDef, INumericalValue numberDividend, INumericalValue numberDivisor)
            => new ExtendedValue(typeDef, numberDividend.AsExtended / numberDivisor.AsExtended);

        internal static IValue Add(ITypeDefinition typeDef, INumericalValue first, INumericalValue second)
            => new ExtendedValue(typeDef, first.AsExtended + second.AsExtended);

        internal static IValue Subtract(ITypeDefinition typeDef, INumericalValue first, INumericalValue second)
            => new ExtendedValue(typeDef, first.AsExtended - second.AsExtended);

        internal static IValue Negate(ITypeDefinition typeDef, INumericalValue value)
            => new ExtendedValue(typeDef, -value.AsExtended);

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
        /// <param name="typeDef"></param>
        /// <returns></returns>
        public static IValue Abs(ITypeDefinition typeDef, INumericalValue floatValue) {
            if (floatValue.IsNegative)
                return new ExtendedValue(typeDef, -floatValue.AsExtended);

            return floatValue;
        }

    }
}
