#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     implementation of the <c>ord</c> routine
    /// </summary>
    public class Ord : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     routine name <c>Ord</c>
        /// </summary>
        public override string Name
            => KnownNames.Ord;

        /// <summary>
        ///     <c>ord</c> routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Ord;

        /// <summary>
        ///     check parameter types
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeSymbol parameter)
            => parameter.TypeDefinition is IOrdinalType;

        /// <summary>
        ///     get the ordinal value
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IValue ExecuteCall(IValue parameter) {
            if (!(parameter is IOrdinalValue value))
                return Integers.Invalid;

            return value.GetOrdinalValue(TypeRegistry);
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ITypeSymbol parameter) {
            if (!(parameter.TypeDefinition is IOrdinalType ordinalType))
                return MakeResult(TypeRegistry.SystemUnit.ErrorType.Reference, parameter);

            ITypeDefinition GetTypeId() {
                var numberOfBytes = ordinalType.TypeSizeInBytes;
                var signed = ordinalType.IsSigned;
                var s = TypeRegistry.SystemUnit;

                if (numberOfBytes > 0 && numberOfBytes <= 1)
                    return signed ? s.ShortIntType : s.ByteType;

                if (numberOfBytes > 1 && numberOfBytes <= 2)
                    return signed ? s.SmallIntType : s.WordType;

                if (numberOfBytes > 2 && numberOfBytes <= 4)
                    return signed ? s.IntegerType : s.CardinalType;

                if (numberOfBytes > 4 && numberOfBytes <= 8)
                    return signed ? s.Int64Type : s.UInt64Type;

                return s.ErrorType;
            };

            return MakeResult(GetTypeId().Reference, parameter);
        }
    }
}
