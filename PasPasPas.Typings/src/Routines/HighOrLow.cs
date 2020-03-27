using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     high or low function mode
    /// </summary>
    public enum HighOrLowMode {

        /// <summary>
        ///     undefined mode
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     high
        /// </summary>
        High = 1,

        /// <summary>
        ///     low
        /// </summary>
        Low = 2

    }

    /// <summary>
    ///     type specification for the <code>High</code> routine
    /// </summary>
    public class HighOrLow : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     create a new high or low function
        /// </summary>
        /// <param name="mode"></param>
        public HighOrLow(HighOrLowMode mode) {
            if (mode == HighOrLowMode.Undefined)
                throw new ArgumentOutOfRangeException(nameof(mode));

            Mode = mode;
        }

        private bool Low
            => Mode == HighOrLowMode.Low;

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => Low ? KnownNames.Low : KnownNames.High;

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     mode
        /// </summary>
        public HighOrLowMode Mode { get; }

        /// <summary>
        ///     procedure kind: function
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId {
            get {
                if (Mode == HighOrLowMode.High)
                    return IntrinsicRoutineId.High;
                else if (Mode == HighOrLowMode.Low)
                    return IntrinsicRoutineId.Low;
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     check parameter types
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter) {

            var typeDefinition = parameter.TypeDefinition;

            if (typeDefinition is IMetaType metaType)
                typeDefinition = metaType.BaseTypeDefinition;

            if (typeDefinition is IOrdinalType)
                return true;

            if (TypeDefinition is IStringType stringType && stringType.Kind == StringTypeKind.ShortString)
                return true;

            if (typeDefinition is IArrayType)
                return true;

            return false;
        }

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter) {

            if (parameter.TypeDefinition is IOrdinalType ordinalType)
                return Low ? ordinalType.LowestElement : ordinalType.HighestElement;

            if (parameter is IShortStringType shortStringType)
                return Low ? Integers.ToScaledIntegerValue(1) : Integers.ToScaledIntegerValue(shortStringType.Size);

            if (parameter.TypeDefinition is IArrayType arrayType &&  //
                arrayType.IndexType is IOrdinalType ordinalIndexType)
                return Low ? ordinalIndexType.LowestElement : ordinalIndexType.HighestElement;

            return Integers.Invalid;
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter)
            => MakeResult(ExecuteCall(parameter as IValue), parameter);
    }
}
