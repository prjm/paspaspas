using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Operators;

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
        ///     get the smallest matching type of two char types
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="typeId1">first type</param>
        /// <param name="typeId2">second type</param>
        /// <returns></returns>
        public static int GetSmallestCharTypeOrNext(this ITypeRegistry registry, int typeId1, int typeId2) {
            if (KnownTypeIds.ErrorType.In(typeId1, typeId2))
                return KnownTypeIds.ErrorType;

            var leftType = registry.GetTypeByIdOrUndefinedType(typeId1);
            var rightType = registry.GetTypeByIdOrUndefinedType(typeId2);

            if (leftType.TypeKind == CommonTypeKind.SubrangeType)
                leftType = registry.GetTypeByIdOrUndefinedType(registry.GetBaseTypeOfSubrangeType(leftType.TypeId));

            if (rightType.TypeKind == CommonTypeKind.SubrangeType)
                rightType = registry.GetTypeByIdOrUndefinedType(registry.GetBaseTypeOfSubrangeType(rightType.TypeId));

            if ((!leftType.TypeKind.IsChar()) || (!rightType.TypeKind.IsChar()))
                return KnownTypeIds.ErrorType;

            var right = rightType as ICharType;

            if (!(leftType is ICharType left) || right == null)
                return KnownTypeIds.ErrorType;

            if (left.BitSize < right.BitSize)
                return typeId2;

            return typeId1;
        }

        /// <summary>
        ///     get the smallest possible boolean type for two types
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="typeId1">first type id</param>
        /// <param name="typeId2">second type id</param>
        /// <param name="minBitSize">minimal bit size</param>
        /// <returns>smallest type id</returns>
        public static int GetSmallestBooleanTypeOrNext(this ITypeRegistry registry, int typeId1, int typeId2, int minBitSize = 0) {
            if (KnownTypeIds.ErrorType.In(typeId1, typeId2))
                return KnownTypeIds.ErrorType;

            var leftType = registry.GetTypeByIdOrUndefinedType(typeId1);
            var rightType = registry.GetTypeByIdOrUndefinedType(typeId2);

            if (leftType.TypeKind == CommonTypeKind.SubrangeType)
                leftType = registry.GetTypeByIdOrUndefinedType(registry.GetBaseTypeOfSubrangeType(leftType.TypeId));

            if (rightType.TypeKind == CommonTypeKind.SubrangeType)
                rightType = registry.GetTypeByIdOrUndefinedType(registry.GetBaseTypeOfSubrangeType(rightType.TypeId));

            if (!CommonTypeKind.BooleanType.All(leftType.TypeKind, rightType.TypeKind))
                return KnownTypeIds.ErrorType;

            var right = rightType as IFixedSizeType;

            if (!(leftType is IFixedSizeType left) || right == null)
                return KnownTypeIds.ErrorType;

            if (left.BitSize < right.BitSize && right.BitSize >= minBitSize)
                return typeId2;

            if (left.BitSize > right.BitSize && left.BitSize >= minBitSize)
                return typeId1;

            if (left.BitSize == right.BitSize && left.BitSize >= minBitSize)
                return left.TypeId;

            if (left.BitSize <= 8 && 16 >= minBitSize)
                return KnownTypeIds.WordBoolType;

            if (left.BitSize <= 16 && 32 >= minBitSize)
                return KnownTypeIds.LongBoolType;

            return KnownTypeIds.ErrorType;
        }

        /// <summary>
        ///     get the smallest possible integral type to for two types
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="typeId1">first type id</param>
        /// <param name="typeId2">second type id</param>
        /// <param name="minBitSize">minimal bit size</param>
        /// <param name="bothAreUnsigned">if <c>true</c> both types reference unsigned values</param>
        /// <returns>smallest type id</returns>
        public static int GetSmallestIntegralTypeOrNext(this ITypeRegistry registry, int typeId1, int typeId2, int minBitSize = 0, bool bothAreUnsigned = false) {
            if (KnownTypeIds.ErrorType.In(typeId1, typeId2))
                return KnownTypeIds.ErrorType;

            var leftType = registry.GetTypeByIdOrUndefinedType(typeId1);
            var rightType = registry.GetTypeByIdOrUndefinedType(typeId2);

            if (leftType.TypeKind == CommonTypeKind.SubrangeType)
                leftType = registry.GetTypeByIdOrUndefinedType(registry.GetBaseTypeOfSubrangeType(leftType.TypeId));

            if (rightType.TypeKind == CommonTypeKind.SubrangeType)
                rightType = registry.GetTypeByIdOrUndefinedType(registry.GetBaseTypeOfSubrangeType(rightType.TypeId));

            var right = rightType as IIntegralType;

            if (!(leftType is IIntegralType left) || right == null)
                return KnownTypeIds.ErrorType;

            var result = default(IIntegralType);

            if (left.BitSize > right.BitSize) {
                result = left;
            }
            else if (left.BitSize == right.BitSize) {
                if (left.Signed == right.Signed)
                    result = left;
                else if (!left.Signed && right.Signed && bothAreUnsigned)
                    result = left;
                else if (left.Signed && !right.Signed && bothAreUnsigned)
                    result = right;
                else if (left.BitSize == 8)
                    result = registry.GetTypeByIdOrUndefinedType(KnownTypeIds.SmallInt) as IIntegralType;
                else if (left.BitSize == 16)
                    result = registry.GetTypeByIdOrUndefinedType(KnownTypeIds.IntegerType) as IIntegralType;
                else if (left.BitSize == 32 || left.BitSize == 64)
                    result = registry.GetTypeByIdOrUndefinedType(KnownTypeIds.Int64Type) as IIntegralType;
                else
                    return KnownTypeIds.ErrorType;
            }
            else {
                result = right;
            }

            if (minBitSize > result.BitSize) {
                if (result.BitSize <= 8 && 16 >= minBitSize)
                    return KnownTypeIds.SmallInt;

                if (result.BitSize <= 16 && 32 >= minBitSize)
                    return KnownTypeIds.IntegerType;

                if (result.BitSize <= 32 && 64 >= minBitSize)
                    return KnownTypeIds.Int64Type;
            }

            return result.TypeId;
        }

        /// <summary>
        ///     helper method: map an expression kind to an registered operator id
        /// </summary>
        /// <param name="typeRegistry">type registry</param>
        /// <param name="kind">expression kind</param>
        /// <param name="left">left type reference</param>
        /// <param name="right">right type reference</param>
        /// <returns>operator id</returns>
        public static int GetOperatorId(this ITypeRegistry typeRegistry, ExpressionKind kind, ITypeReference left, ITypeReference right) {
            var leftType = left.TypeKind;
            var rightType = right.TypeKind;

            if (leftType == CommonTypeKind.UnknownType || rightType == CommonTypeKind.UnknownType)
                return DefinedOperators.Undefined;

            switch (kind) {
                case ExpressionKind.LessThen:
                    return DefinedOperators.LessThen;
                case ExpressionKind.LessThenEquals:
                    return DefinedOperators.LessThenOrEqual;
                case ExpressionKind.GreaterThen:
                    return DefinedOperators.GreaterThen;
                case ExpressionKind.GreaterThenEquals:
                    return DefinedOperators.GreaterThenEqual;
                case ExpressionKind.NotEquals:
                    return DefinedOperators.NotEqualsOperator;
                case ExpressionKind.EqualsSign:
                    return DefinedOperators.EqualsOperator;
                case ExpressionKind.Xor:
                    return DefinedOperators.XorOperator;
                case ExpressionKind.Or:
                    return DefinedOperators.OrOperator;
                case ExpressionKind.Minus:
                    return DefinedOperators.MinusOperator;
                case ExpressionKind.Shr:
                    return DefinedOperators.ShrOperator;
                case ExpressionKind.Shl:
                    return DefinedOperators.ShlOperator;
                case ExpressionKind.And:
                    return DefinedOperators.AndOperator;
                case ExpressionKind.Mod:
                    return DefinedOperators.ModOperator;
                case ExpressionKind.Slash:
                    return DefinedOperators.SlashOperator;
                case ExpressionKind.Times:
                    return DefinedOperators.TimesOperator;
                case ExpressionKind.Div:
                    return DefinedOperators.DivOperator;
                case ExpressionKind.Not:
                    return DefinedOperators.NotOperator;
                case ExpressionKind.UnaryMinus:
                    return DefinedOperators.UnaryMinus;
                case ExpressionKind.UnaryPlus:
                    return DefinedOperators.UnaryPlus;
            };

            if (kind == ExpressionKind.Plus) {

                if (leftType.IsTextual() && rightType.IsTextual())
                    return DefinedOperators.ConcatOperator;

                if (leftType.IsNumerical() && rightType.IsNumerical())
                    return DefinedOperators.PlusOperator;

                if (leftType == CommonTypeKind.SubrangeType)
                    return typeRegistry.GetOperatorId(kind, typeRegistry.MakeReference(typeRegistry.GetBaseTypeOfSubrangeType(left.TypeId)), right);

                if (rightType == CommonTypeKind.SubrangeType)
                    return typeRegistry.GetOperatorId(kind, left, typeRegistry.MakeReference(typeRegistry.GetBaseTypeOfSubrangeType(right.TypeId)));

            }

            return DefinedOperators.Undefined;
        }

        /// <summary>
        ///     determine the resulting type for a subrange type
        /// </summary>
        /// <param name="types">used type registry</param>
        /// <param name="lowerBound">lower bound of the subrange</param>
        /// <param name="upperBound">upper bound of the subrange type</param>
        /// <returns></returns>
        public static int GetTypeForSubrangeType(this ITypeRegistry types, ITypeReference lowerBound, ITypeReference upperBound) {
            var left = lowerBound.TypeKind;
            var right = upperBound.TypeKind;

            if (!lowerBound.IsConstant || !upperBound.IsConstant)
                return KnownTypeIds.ErrorType;

            if (left.IsOrdinal() && right.IsOrdinal()) {

                var isIntegral = left.IsIntegral() && right.IsIntegral();
                var isChar = left.IsChar() && right.IsChar();
                var isBoolean = left == CommonTypeKind.BooleanType && right == CommonTypeKind.BooleanType;
                var isEnum = left == CommonTypeKind.EnumerationType && right == CommonTypeKind.EnumerationType;

                if (isIntegral || isChar || isBoolean || isEnum) {

                    if (types.Runtime.IsValueGreaterThen(lowerBound, upperBound)) // lower bound > upper bound?
                        return KnownTypeIds.ErrorType;

                    var bothUnsinged = types.Runtime.AreValuesUnsigned(lowerBound, upperBound);
                    var baseType = KnownTypeIds.ErrorType;

                    if (isIntegral)
                        baseType = types.GetSmallestIntegralTypeOrNext(lowerBound.TypeId, upperBound.TypeId, 8, bothUnsinged);
                    else if (isChar)
                        baseType = types.GetSmallestCharTypeOrNext(lowerBound.TypeId, upperBound.TypeId);
                    else if (isBoolean)
                        baseType = types.GetSmallestBooleanTypeOrNext(lowerBound.TypeId, upperBound.TypeId);
                    else if (isEnum && lowerBound.TypeId == upperBound.TypeId)
                        baseType = lowerBound.TypeId;

                    var typeDef = types.RegisterType(new Simple.SubrangeType(types.RequireUserTypeId(), baseType));
                    return typeDef.TypeId;
                }

                if (lowerBound.TypeId == upperBound.TypeId) {
                    var baseType = types.GetTypeByIdOrUndefinedType(upperBound.TypeId);
                    var typeDef = types.RegisterType(new Simple.SubrangeType(types.RequireUserTypeId(), baseType.TypeId));
                    return typeDef.TypeId;
                }
            }

            return KnownTypeIds.ErrorType;
        }
    }
}
