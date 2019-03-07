using System;
using PasPasPas.Globals.Runtime;

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
            => Low ? "Low" : "High";

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
        ///     check parameter types
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter) {

            var typeKind = parameter.TypeKind;

            if (typeKind.IsType())
                typeKind = TypeRegistry.GetTypeKindOf(parameter.TypeId);

            if (typeKind.IsOrdinal())
                return true;

            if (typeKind.IsShortString())
                return true;

            if (typeKind.IsArray())
                return true;

            return false;
        }

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {

            if (IsOrdinalType(parameter.TypeId, out var ordinalType))
                return Low ? ordinalType.LowestElement : ordinalType.HighestElement;

            if (IsShortStringType(parameter.TypeId, out var shortStringType))
                return Low ? Integers.ToScaledIntegerValue(1) : shortStringType.Size;

            if (IsArrayType(parameter.TypeId, out var arrayType) &&  //
                IsOrdinalType(arrayType.IndexTypeId, out var ordinalIndexType))
                return Low ? ordinalIndexType.LowestElement : ordinalIndexType.HighestElement;

            return RuntimeException();
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => ExecuteCall(parameter);
    }
}
