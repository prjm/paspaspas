using PasPasPas.Global.Constants;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     helper class for type registry
    /// </summary>
    public static class RegisteredTypesHelper {

        /// <summary>
        ///     get the type kind for a given type id
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="typeId">type id</param>
        /// <returns>type kind</returns>
        public static CommonTypeKind GetTypeKind(this ITypeRegistry registry, int typeId)
            => registry.GetTypeByIdOrUndefinedType(typeId).TypeKind;

        /// <summary>
        ///     get the smallest possible integral type to for two types
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="typeId1">first type id</param>
        /// <param name="typeId2">second type id</param>
        /// <param name="minBitSize">minimal bit size</param>
        /// <returns>smallest type id</returns>
        public static int GetSmallestIntegralTypeOrNext(this ITypeRegistry registry, int typeId1, int typeId2, int minBitSize = 0) {
            if (KnownTypeIds.ErrorType.In(typeId1, typeId2))
                return KnownTypeIds.ErrorType;

            var left = registry.GetTypeByIdOrUndefinedType(typeId1) as IIntegralType;
            var right = registry.GetTypeByIdOrUndefinedType(typeId2) as IIntegralType;

            if (left == null || right == null)
                return KnownTypeIds.ErrorType;

            if (left.BitSize < right.BitSize && right.BitSize >= minBitSize)
                return typeId2;

            if (left.BitSize > right.BitSize && left.BitSize >= minBitSize)
                return typeId1;

            if (left.Signed == right.Signed && left.BitSize == right.BitSize && left.BitSize >= minBitSize)
                return left.TypeInfo.TypeId;

            if (left.BitSize <= 8 && 16 >= minBitSize)
                return KnownTypeIds.SmallInt;

            if (left.BitSize <= 16 && 32 >= minBitSize)
                return KnownTypeIds.IntegerType;

            if (left.BitSize <= 32 && 64 >= minBitSize)
                return KnownTypeIds.Int64Type;

            return KnownTypeIds.ErrorType;
        }

    }
}
