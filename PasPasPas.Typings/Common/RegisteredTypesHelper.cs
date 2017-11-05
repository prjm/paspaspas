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
        ///     get the smallest possibe integral type to express two types
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="typeId1">first type id</param>
        /// <param name="typeId2">second type id</param>
        /// <returns>smalles type id</returns>
        public static int GetSmallestIntegralTypeOrNext(this ITypeRegistry registry, int typeId1, int typeId2) {
            if (TypeIds.ErrorType.In(typeId1, typeId2))
                return TypeIds.ErrorType;

            var left = registry.GetTypeByIdOrUndefinedType(typeId1) as IIntegralType;
            var right = registry.GetTypeByIdOrUndefinedType(typeId2) as IIntegralType;

            if (left == null || right == null)
                return TypeIds.ErrorType;

            if (left.BitSize < right.BitSize)
                return typeId2;
            else if (left.BitSize > right.BitSize)
                return typeId1;

            if (left.Signed == right.Signed)
                return left.TypeId;

            if (left.BitSize == 8)
                return TypeIds.SmallInt;

            if (left.BitSize == 16)
                return TypeIds.IntegerType;

            if (left.BitSize == 32)
                return TypeIds.Int64Type;

            return TypeIds.ErrorType;
        }

    }
}
