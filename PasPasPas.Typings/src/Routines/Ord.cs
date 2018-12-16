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
        ///     routine name <c>Ord</c>
        /// </summary>
        public override string Name
            => "Ord";

        /// <summary>
        ///     check parameter types
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter)
            => parameter.IsOrdinal();

        /// <summary>
        ///     get the ordinal value
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {
            if (!parameter.IsOrdinalValue(out var value))
                return RuntimeException();

            return value.GetOrdinalValue(TypeRegistry);
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter) {
            if (!IsOrdinalType(parameter.TypeId, out var ordinalType))
                return Runtime.Types.MakeErrorTypeReference();

            int GetTypeId() {
                var bitSize = ordinalType.BitSize;
                var signed = ordinalType.IsSigned;

                if (bitSize > 0 && bitSize <= 8)
                    return signed ? KnownTypeIds.ShortInt : KnownTypeIds.ByteType;

                if (bitSize > 8 && bitSize <= 16)
                    return signed ? KnownTypeIds.SmallInt : KnownTypeIds.WordType;

                if (bitSize > 16 && bitSize <= 32)
                    return signed ? KnownTypeIds.IntegerType : KnownTypeIds.CardinalType;

                if (bitSize > 32 && bitSize <= 64)
                    return signed ? KnownTypeIds.Int64Type : KnownTypeIds.Uint64Type;

                return KnownTypeIds.ErrorType;
            };

            return MakeTypeInstanceReference(GetTypeId());
        }
    }
}
