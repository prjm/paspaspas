using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Operators;
using PasPasPas.Typings.Structured;

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
        ///     test if the given type is a subrange type
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="subrangeType"></param>
        /// <param name="typeRegistry"></param>
        /// <returns></returns>
        public static bool IsSubrangeType(this ITypeRegistry typeRegistry, int typeId, out ISubrangeType subrangeType) {
            subrangeType = typeRegistry.GetTypeByIdOrUndefinedType(typeId) as ISubrangeType;

            if (subrangeType != default && subrangeType.TypeKind == CommonTypeKind.SubrangeType)
                return true;

            subrangeType = default;
            return false;
        }

        /// <summary>
        ///     resolve a type alias
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static ITypeDefinition ResolveAlias(this ITypeRegistry typeRegistry, int typeId) {
            var typeDef = typeRegistry.GetTypeByIdOrUndefinedType(typeId);
            return TypeBase.ResolveAlias(typeDef);
        }

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

            if (registry.IsSubrangeType(typeId1, out var subrangeType1))
                leftType = subrangeType1.BaseType;

            if (registry.IsSubrangeType(typeId2, out var subrangeType2))
                rightType = subrangeType2.BaseType;

            if (!leftType.TypeKind.IsChar() || !rightType.TypeKind.IsChar())
                return KnownTypeIds.ErrorType;

            var right = rightType as ICharType;

            if (!(leftType is ICharType left) || right == null)
                return KnownTypeIds.ErrorType;

            if (left.BitSize < right.BitSize)
                return typeId2;

            return typeId1;
        }

        /// <summary>
        ///     get the smallest text type of two char types
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="typeId1">first type</param>
        /// <param name="typeId2">second type</param>
        /// <returns></returns>
        public static int GetSmallestTextTypeOrNext(this ITypeRegistry registry, int typeId1, int typeId2) {
            if (KnownTypeIds.ErrorType.In(typeId1, typeId2))
                return KnownTypeIds.ErrorType;

            var leftType = registry.GetTypeByIdOrUndefinedType(typeId1);
            var rightType = registry.GetTypeByIdOrUndefinedType(typeId2);

            if (registry.IsSubrangeType(typeId1, out var subrangeType1))
                leftType = subrangeType1.BaseType;

            if (registry.IsSubrangeType(typeId2, out var subrangeType2))
                rightType = subrangeType2.BaseType;

            if (!leftType.TypeKind.IsTextual())
                return KnownTypeIds.ErrorType;

            if (!rightType.TypeKind.IsTextual())
                return KnownTypeIds.ErrorType;

            if (leftType.TypeKind.IsChar() && rightType.TypeKind.IsChar())
                return GetSmallestCharTypeOrNext(registry, leftType.TypeId, rightType.TypeId);

            if (leftType.TypeKind == CommonTypeKind.UnicodeStringType)
                return leftType.TypeId;

            if (rightType.TypeKind == CommonTypeKind.UnicodeStringType)
                return rightType.TypeId;

            if (leftType.TypeKind.IsAnsiString() && rightType.TypeKind.IsAnsiString()) {
                return KnownTypeIds.ErrorType;
            }

            if (leftType.TypeKind.IsString())
                return leftType.TypeId;

            if (rightType.TypeKind.IsString())
                return rightType.TypeId;

            return KnownTypeIds.UnicodeStringType;
        }

        /// <summary>
        ///     test if two record types are compatible
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="typeId1"></param>
        /// <param name="typeId2"></param>
        /// <returns></returns>
        public static bool AreRecordTypesCompatible(this ITypeRegistry registry, int typeId1, int typeId2) {
            var leftType = registry.ResolveAlias(typeId1);
            var rightType = registry.ResolveAlias(typeId2);

            if (!(leftType is StructuredTypeDeclaration leftStruct) || leftType.TypeKind != CommonTypeKind.RecordType)
                return false;

            if (!(rightType is StructuredTypeDeclaration rightStruct) || rightType.TypeKind != CommonTypeKind.RecordType)
                return false;

            if (leftStruct.Fields.Count != rightStruct.Fields.Count)
                return false;

            for (var i = 0; i < leftStruct.Fields.Count; i++) {
                var leftField = leftStruct.Fields[i].SymbolType;
                var rightField = rightStruct.Fields[i].SymbolType;

                var leftTypeDef = registry.GetTypeByIdOrUndefinedType(leftField.TypeId);
                var rightTypeDef = registry.GetTypeByIdOrUndefinedType(rightField.TypeId);

                if (!rightTypeDef.CanBeAssignedFrom(leftTypeDef))
                    return false;

            }

            return true;
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

            if (registry.IsSubrangeType(typeId1, out var subrangeType1))
                leftType = subrangeType1.BaseType;

            if (registry.IsSubrangeType(typeId2, out var subrangeType2))
                rightType = subrangeType2.BaseType;

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

            if (registry.IsSubrangeType(typeId1, out var subrangeType1))
                leftType = subrangeType1.BaseType;

            if (registry.IsSubrangeType(typeId2, out var subrangeType2))
                rightType = subrangeType2.BaseType;

            var right = rightType as IIntegralType;

            if (!(leftType is IIntegralType left) || right == null)
                return KnownTypeIds.ErrorType;

            var result = default(IIntegralType);

            if (left.BitSize > right.BitSize) {
                result = left;
            }
            else if (left.BitSize == right.BitSize) {
                if (left.IsSigned == right.IsSigned)
                    result = left;
                else if (!left.IsSigned && right.IsSigned && bothAreUnsigned)
                    result = left;
                else if (left.IsSigned && !right.IsSigned && bothAreUnsigned)
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

            if (typeRegistry.IsSubrangeType(left.TypeId, out var subrangeType1))
                return typeRegistry.GetOperatorId(kind, typeRegistry.MakeTypeInstanceReference(subrangeType1.BaseType.TypeId), right);

            if (typeRegistry.IsSubrangeType(right.TypeId, out var subrangeType2))
                return typeRegistry.GetOperatorId(kind, left, typeRegistry.MakeTypeInstanceReference(subrangeType2.BaseType.TypeId));

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
                case ExpressionKind.Div:
                    return DefinedOperators.DivOperator;
                case ExpressionKind.Not:
                    return DefinedOperators.NotOperator;
                case ExpressionKind.UnaryMinus:
                    return DefinedOperators.UnaryMinus;
                case ExpressionKind.UnaryPlus:
                    return DefinedOperators.UnaryPlus;
                case ExpressionKind.In:
                    return DefinedOperators.InSetOperator;
                case ExpressionKind.As:
                    return DefinedOperators.AsOperator;
                case ExpressionKind.Is:
                    return DefinedOperators.IsOperator;
            };

            if (kind == ExpressionKind.Plus) {

                if (leftType.IsTextual() && rightType.IsTextual())
                    return DefinedOperators.ConcatOperator;

                if (leftType.IsNumerical() && rightType.IsNumerical())
                    return DefinedOperators.PlusOperator;

                if (leftType.IsSet() && rightType.IsSet())
                    return DefinedOperators.SetAddOperator;

                if (leftType.IsArray() && rightType.IsArray())
                    return DefinedOperators.ConcatArrayOperator;

            }

            if (kind == ExpressionKind.Minus) {

                if (leftType.IsNumerical() && rightType.IsNumerical())
                    return DefinedOperators.MinusOperator;

                if (leftType.IsSet() && rightType.IsSet())
                    return DefinedOperators.SetDifferenceOperator;

            }

            if (kind == ExpressionKind.Times) {

                if (leftType.IsNumerical() && rightType.IsNumerical())
                    return DefinedOperators.TimesOperator;

                if (leftType.IsSet() && rightType.IsSet())
                    return DefinedOperators.SetIntersectOperator;

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

            if (!lowerBound.IsConstant() || !upperBound.IsConstant())
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

                    if (baseType == KnownTypeIds.ErrorType)
                        return baseType;

                    var typeDef = types.TypeCreator.CreateSubrangeType(baseType, lowerBound, upperBound);
                    return typeDef.TypeId;
                }

                if (lowerBound.TypeId == upperBound.TypeId) {
                    var baseType = types.GetTypeByIdOrUndefinedType(upperBound.TypeId);
                    var typeDef = types.TypeCreator.CreateSubrangeType(baseType.TypeId, lowerBound, upperBound);
                    return typeDef.TypeId;
                }
            }

            return KnownTypeIds.ErrorType;
        }

        /// <summary>
        ///     determine a base type for an array or set type
        /// </summary>
        /// <param name="types"></param>
        /// <param name="baseType"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static ITypeReference GetBaseTypeForArrayOrSet(this ITypeRegistry types, ITypeReference baseType, ITypeReference elementType) {
            if (elementType == default)
                baseType = types.Runtime.Types.MakeErrorTypeReference();
            else if (baseType == default)
                baseType = types.MakeTypeInstanceReference(elementType.TypeId);
            else if (baseType.TypeKind.IsIntegral() && elementType.TypeKind.IsIntegral())
                baseType = types.MakeTypeInstanceReference(types.GetSmallestIntegralTypeOrNext(baseType.TypeId, elementType.TypeId));
            else if (baseType.TypeKind.IsTextual() && elementType.TypeKind.IsTextual())
                baseType = types.MakeTypeInstanceReference(types.GetSmallestTextTypeOrNext(baseType.TypeId, elementType.TypeId));
            else if (baseType.TypeKind.IsOrdinal() && baseType.TypeId == elementType.TypeId)
                baseType = types.MakeTypeInstanceReference(elementType.TypeId);
            else if (baseType.TypeKind == CommonTypeKind.RealType && elementType.TypeKind == CommonTypeKind.RealType)
                baseType = types.MakeTypeInstanceReference(KnownTypeIds.Extended);
            else if (baseType.TypeKind == CommonTypeKind.RecordType && types.AreRecordTypesCompatible(baseType.TypeId, elementType.TypeId))
                baseType = types.MakeTypeInstanceReference(elementType.TypeId);

            return baseType;
        }

        /// <summary>
        ///     generate a new set type definition
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static ITypeReference GetMatchingSetType(this ITypeRegistry types, ITypeReference left, ITypeReference right) {
            var baseType = types.GetMatchingSetBaseType(left, right, out var newType);
            if (baseType == KnownTypeIds.ErrorType)
                return types.Runtime.Types.MakeErrorTypeReference();

            if (!newType) {
                return types.MakeTypeInstanceReference(left.TypeId);
            }

            var type = types.TypeCreator.CreateSetType(baseType);
            return types.MakeTypeInstanceReference(type.TypeId);
        }

        /// <summary>
        ///     check if the sets have a compatible base type
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool HaveSetsCommonBaseType(this ITypeRegistry typeRegistry, ITypeReference left, ITypeReference right) {
            var baseType = typeRegistry.GetMatchingSetBaseType(left, right, out var _);
            return baseType != KnownTypeIds.ErrorType;
        }

        /// <summary>
        ///     find a matching set base type
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="typeRegistry"></param>
        /// <param name="requireNewType"></param>
        /// <returns></returns>
        public static int GetMatchingSetBaseType(this ITypeRegistry typeRegistry, ITypeReference left, ITypeReference right, out bool requireNewType) {
            requireNewType = false;

            if (!(typeRegistry.GetTypeByIdOrUndefinedType(left.TypeId) is ISetType leftType))
                return KnownTypeIds.ErrorType;

            if (!(typeRegistry.GetTypeByIdOrUndefinedType(right.TypeId) is ISetType rightType))
                return KnownTypeIds.ErrorType;

            if (!(typeRegistry.ResolveAlias(leftType.BaseTypeId) is IOrdinalType leftBaseType))
                return KnownTypeIds.ErrorType;

            if (!(typeRegistry.ResolveAlias(rightType.BaseTypeId) is IOrdinalType rightBaseType))
                return KnownTypeIds.ErrorType;

            if (leftBaseType.TypeId == rightBaseType.TypeId)
                return leftBaseType.TypeId;

            if (leftBaseType is ISubrangeType lsrt && typeRegistry.ResolveAlias(lsrt.BaseTypeId) is IOrdinalType lsrtb)
                leftBaseType = lsrtb;

            if (rightBaseType is ISubrangeType rsrt && typeRegistry.ResolveAlias(rsrt.BaseTypeId) is IOrdinalType rsrtb)
                leftBaseType = rsrtb;

            if (leftBaseType.TypeId == rightBaseType.TypeId)
                return leftBaseType.TypeId;

            if (leftBaseType is IIntegralType && rightBaseType is IIntegralType) {
                var result = typeRegistry.GetSmallestIntegralTypeOrNext(leftBaseType.TypeId, rightBaseType.TypeId);
                requireNewType = result != leftBaseType.TypeId || result != rightBaseType.TypeId;
                return result;
            }

            if (leftBaseType is ICharType && rightBaseType is ICharType) {
                var result = typeRegistry.GetSmallestCharTypeOrNext(leftBaseType.TypeId, rightBaseType.TypeId);
                requireNewType = result != leftBaseType.TypeId || result != rightBaseType.TypeId;
                return result;
            }
            if (leftBaseType is IBooleanType && rightBaseType is IBooleanType) {
                var result = typeRegistry.GetSmallestBooleanTypeOrNext(leftBaseType.TypeId, rightBaseType.TypeId);
                requireNewType = result != leftBaseType.TypeId || result != rightBaseType.TypeId;
                return result;
            }

            return KnownTypeIds.ErrorType;
        }

        /// <summary>
        ///     get the size of a generic pointer
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <returns></returns>
        public static uint GetPointerSize(this ITypeRegistry typeRegistry)
            => typeRegistry.GetTypeByIdOrUndefinedType(KnownTypeIds.GenericPointer).TypeSizeInBytes;

        /// <summary>
        ///     check if two types have a common base class
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="leftTypeId"></param>
        /// <param name="rightTypeId"></param>
        /// <returns></returns>
        public static bool AreCommonBaseClasses(this ITypeRegistry typeRegistry, int leftTypeId, int rightTypeId) {
            var leftClass = typeRegistry.GetTypeByIdOrUndefinedType(leftTypeId) as IStructuredType;
            var rightClass = typeRegistry.GetTypeByIdOrUndefinedType(rightTypeId) as IStructuredType;

            if (leftClass.TypeId == rightClass.TypeId)
                return true;

            var metaBaseClass = typeRegistry.GetTypeByIdOrUndefinedType(leftClass.BaseClass?.TypeId ?? KnownTypeIds.ErrorType) as IMetaStructuredType;
            while (metaBaseClass != default) {
                var baseClass = typeRegistry.GetTypeByIdOrUndefinedType(metaBaseClass.BaseType) as IStructuredType;
                if (baseClass.TypeId == rightClass.TypeId)
                    return true;
                metaBaseClass = typeRegistry.GetTypeByIdOrUndefinedType(baseClass.BaseClass?.TypeId ?? KnownTypeIds.ErrorType) as IMetaStructuredType;
            }

            metaBaseClass = typeRegistry.GetTypeByIdOrUndefinedType(rightClass.BaseClass?.TypeId ?? KnownTypeIds.ErrorType) as IMetaStructuredType;
            while (metaBaseClass != default) {
                var baseClass = typeRegistry.GetTypeByIdOrUndefinedType(metaBaseClass.BaseType) as IStructuredType;
                if (baseClass.TypeId == leftClass.TypeId)
                    return true;
                metaBaseClass = typeRegistry.GetTypeByIdOrUndefinedType(baseClass.BaseClass?.TypeId ?? KnownTypeIds.ErrorType) as IMetaStructuredType;
            }


            return false;
        }

    }
}
