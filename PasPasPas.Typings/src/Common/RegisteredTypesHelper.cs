using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Operators;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     helper class for type registry
    /// </summary>
    public static class RegisteredTypesHelper {

        /// <summary>
        ///     test if a given type is a string type
        /// </summary>
        /// <param name="typeDefinition"></param>
        /// <param name="stringTypeDefinition"></param>
        /// <returns></returns>
        public static bool IsStringType(this ITypeDefinition typeDefinition, out IStringType stringTypeDefinition) {
            if (typeDefinition.BaseType == BaseType.String && typeDefinition is IStringType stringType) {
                stringTypeDefinition = stringType;
                return true;
            }

            stringTypeDefinition = default;
            return false;
        }

        /// <summary>
        ///     resolve a type alias
        /// </summary>
        /// <param name="typeDefinition"></param>
        /// <returns></returns>
        public static ITypeDefinition ResolveAlias(this ITypeDefinition typeDefinition) {
            if (typeDefinition.BaseType == BaseType.TypeAlias && typeDefinition is IAliasedType alias)
                return alias.BaseTypeDefinition.ResolveAlias();
            else
                return typeDefinition;
        }

        /// <summary>
        ///     check if this type definition is an error type
        /// </summary>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        public static bool IsErrorType(this ITypeDefinition typeDef) {
            if (typeDef.BaseType == BaseType.Error)
                return true;

            if (typeDef.BaseType == BaseType.Unkown)
                return true;

            if (typeDef.BaseType == BaseType.Hidden)
                return true;

            return false;
        }

        /// <summary>
        ///     get the smallest matching type of two char types
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="typeId1">first type</param>
        /// <param name="typeId2">second type</param>
        /// <returns></returns>
        public static ITypeDefinition GetSmallestCharTypeOrNext(this ITypeRegistry registry, ITypeDefinition typeId1, ITypeDefinition typeId2) {

            if (typeId1.IsErrorType() || typeId2.IsErrorType())
                return registry.SystemUnit.ErrorType;

            if (typeId1.IsSubrangeType(out var subrangeType1))
                typeId1 = subrangeType1.SubrangeOfType;

            if (typeId2.IsSubrangeType(out var subrangeType2))
                typeId2 = subrangeType2.SubrangeOfType;

            if (typeId1.BaseType != BaseType.Char || typeId2.BaseType != BaseType.Char)
                return registry.SystemUnit.ErrorType;

            var right = typeId2 as ICharType;

            if (!(typeId1 is ICharType left) || right == null)
                return registry.SystemUnit.ErrorType;

            if (left.TypeSizeInBytes < right.TypeSizeInBytes)
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
        public static ITypeDefinition GetSmallestTextTypeOrNext(this ITypeRegistry registry, ITypeDefinition typeId1, ITypeDefinition typeId2) {
            if (typeId1.IsErrorType() || typeId2.IsErrorType())
                return registry.SystemUnit.ErrorType;

            if (typeId1.IsSubrangeType(out var subrangeType1))
                typeId1 = subrangeType1.SubrangeOfType;

            if (typeId2.IsSubrangeType(out var subrangeType2))
                typeId2 = subrangeType2.SubrangeOfType;

            if (typeId1.BaseType != BaseType.Char && typeId1.BaseType != BaseType.String)
                return registry.SystemUnit.ErrorType;

            if (typeId2.BaseType != BaseType.Char && typeId2.BaseType != BaseType.String)
                return registry.SystemUnit.ErrorType;

            if (typeId1.BaseType == BaseType.Char && typeId2.BaseType == BaseType.Char)
                return GetSmallestCharTypeOrNext(registry, typeId1, typeId2);

            if (typeId1.IsStringType(out var leftString) && leftString.Kind == StringTypeKind.UnicodeString)
                return leftString;

            if (typeId2.IsStringType(out var rightString) && rightString.Kind == StringTypeKind.UnicodeString)
                return rightString;

            if (typeId1 is IAnsiStringType ansiString1 && typeId2 is IAnsiStringType ansiString2) {

                if (ansiString1.WithCodePage == ansiString2.WithCodePage)
                    return ansiString1;

                return registry.SystemUnit.ErrorType;
            }

            return registry.SystemUnit.UnicodeStringType;
        }

        /// <summary>
        ///     test if two record types are compatible
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="typeId1"></param>
        /// <param name="typeId2"></param>
        /// <returns></returns>
        public static bool AreRecordTypesCompatible(this ITypeRegistry registry, ITypeDefinition typeId1, ITypeDefinition typeId2) {
            var leftType = typeId1.ResolveAlias();
            var rightType = typeId2.ResolveAlias();

            if (!(leftType is StructuredTypeDeclaration leftStruct) || leftType.BaseType != BaseType.Structured)
                return false;

            if (!(rightType is StructuredTypeDeclaration rightStruct) || rightType.BaseType != BaseType.Structured)
                return false;

            if (leftStruct.StructTypeKind != StructuredTypeKind.Record)
                return false;

            if (rightStruct.StructTypeKind != StructuredTypeKind.Record)
                return false;

            if (leftStruct.Fields.Count != rightStruct.Fields.Count)
                return false;

            for (var i = 0; i < leftStruct.Fields.Count; i++) {
                var leftField = leftStruct.Fields[i];
                var rightField = rightStruct.Fields[i];

                var leftTypeDef = leftField.TypeDefinition;
                var rightTypeDef = rightField.TypeDefinition;

                if (!leftTypeDef.CanBeAssignedFromType(rightTypeDef))
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
        /// <param name="minByteSize">minimal bit size</param>
        /// <returns>smallest type id</returns>
        public static ITypeDefinition GetSmallestBooleanTypeOrNext(this ITypeRegistry registry, ITypeDefinition typeId1, ITypeDefinition typeId2, int minByteSize = 0) {
            if (typeId1.IsErrorType() || typeId2.IsErrorType())
                return registry.SystemUnit.ErrorType;

            if (typeId1.IsSubrangeType(out var subrangeType1))
                typeId1 = subrangeType1.SubrangeOfType;

            if (typeId2.IsSubrangeType(out var subrangeType2))
                typeId2 = subrangeType2.SubrangeOfType;

            if (typeId1.BaseType != BaseType.Boolean || typeId2.BaseType != BaseType.Boolean)
                return registry.SystemUnit.ErrorType;

            var right = typeId2 as IFixedSizeType;

            if (!(typeId1 is IFixedSizeType left) || right == null)
                return registry.SystemUnit.ErrorType;

            if (left.TypeSizeInBytes < right.TypeSizeInBytes && right.TypeSizeInBytes >= minByteSize)
                return typeId2;

            if (left.TypeSizeInBytes > right.TypeSizeInBytes && left.TypeSizeInBytes >= minByteSize)
                return typeId1;

            if (left.TypeSizeInBytes == right.TypeSizeInBytes && left.TypeSizeInBytes >= minByteSize)
                return left;

            if (left.TypeSizeInBytes <= 1 && 2 >= minByteSize)
                return registry.SystemUnit.WordBoolType;

            if (left.TypeSizeInBytes <= 2 && 4 >= minByteSize)
                return registry.SystemUnit.LongBoolType;

            return registry.SystemUnit.ErrorType;
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
        public static ITypeDefinition GetSmallestIntegralTypeOrNext(this ITypeRegistry registry, ITypeDefinition typeId1, ITypeDefinition typeId2, int minBitSize = 0, bool bothAreUnsigned = false) {
            if (typeId1.IsErrorType() || typeId2.IsErrorType())
                return registry.SystemUnit.ErrorType;

            if (typeId1.IsSubrangeType(out var subrangeType1))
                typeId1 = subrangeType1.SubrangeOfType;

            if (typeId2.IsSubrangeType(out var subrangeType2))
                typeId2 = subrangeType2.SubrangeOfType;

            var right = typeId2 as IIntegralType;

            if (!(typeId1 is IIntegralType left) || right == null)
                return registry.SystemUnit.ErrorType;

            var result = default(IIntegralType);

            if (left.TypeSizeInBytes > right.TypeSizeInBytes) {
                result = left;
            }
            else if (left.TypeSizeInBytes == right.TypeSizeInBytes) {
                if (left.IsSigned == right.IsSigned)
                    result = left;
                else if (!left.IsSigned && right.IsSigned && bothAreUnsigned)
                    result = left;
                else if (left.IsSigned && !right.IsSigned && bothAreUnsigned)
                    result = right;
                else if (left.TypeSizeInBytes == 1)
                    result = registry.SystemUnit.SmallIntType;
                else if (left.TypeSizeInBytes == 2)
                    result = registry.SystemUnit.IntegerType;
                else if (left.TypeSizeInBytes == 4 || left.TypeSizeInBytes == 8)
                    result = registry.SystemUnit.Int64Type;
                else
                    return registry.SystemUnit.ErrorType;
            }
            else {
                result = right;
            }

            if (minBitSize > result.TypeSizeInBytes) {
                if (result.TypeSizeInBytes <= 1 && 2 >= minBitSize)
                    return registry.SystemUnit.SmallIntType;

                if (result.TypeSizeInBytes <= 2 && 4 >= minBitSize)
                    return registry.SystemUnit.IntegerType;

                if (result.TypeSizeInBytes <= 4 && 8 >= minBitSize)
                    return registry.SystemUnit.Int64Type;
            }

            return result;
        }

        /// <summary>
        ///     helper method: map an expression kind to an registered operator id
        /// </summary>
        /// <param name="typeRegistry">type registry</param>
        /// <param name="kind">expression kind</param>
        /// <param name="left">left type reference</param>
        /// <param name="right">right type reference</param>
        /// <returns>operator id</returns>
        public static OperatorKind GetOperatorId(this ITypeRegistry typeRegistry, ExpressionKind kind, ITypeSymbol left, ITypeSymbol right) {
            var leftType = left.TypeDefinition;
            var rightType = right.TypeDefinition;

            if (leftType.IsErrorType() || rightType.IsErrorType())
                return OperatorKind.Undefined;

            if (leftType.IsSubrangeType(out var subrangeType1))
                leftType = subrangeType1.SubrangeOfType;

            if (rightType.IsSubrangeType(out var subrangeType2))
                rightType = subrangeType2.SubrangeOfType;

            switch (kind) {
                case ExpressionKind.LessThen:
                    return OperatorKind.LessThan;
                case ExpressionKind.LessThenEquals:
                    return OperatorKind.LessThanOrEqual;
                case ExpressionKind.GreaterThen:
                    return OperatorKind.GreaterThan;
                case ExpressionKind.GreaterThenEquals:
                    return OperatorKind.GreaterThanOrEqual;
                case ExpressionKind.NotEquals:
                    return OperatorKind.NotEqualsOperator;
                case ExpressionKind.EqualsSign:
                    return OperatorKind.EqualsOperator;
                case ExpressionKind.Xor:
                    return OperatorKind.XorOperator;
                case ExpressionKind.Or:
                    return OperatorKind.OrOperator;
                case ExpressionKind.Shr:
                    return OperatorKind.ShrOperator;
                case ExpressionKind.Shl:
                    return OperatorKind.ShlOperator;
                case ExpressionKind.And:
                    return OperatorKind.AndOperator;
                case ExpressionKind.Mod:
                    return OperatorKind.ModOperator;
                case ExpressionKind.Slash:
                    return OperatorKind.SlashOperator;
                case ExpressionKind.Div:
                    return OperatorKind.DivOperator;
                case ExpressionKind.Not:
                    return OperatorKind.NotOperator;
                case ExpressionKind.UnaryMinus:
                    return OperatorKind.UnaryMinus;
                case ExpressionKind.UnaryPlus:
                    return OperatorKind.UnaryPlus;
                case ExpressionKind.In:
                    return OperatorKind.InSetOperator;
                case ExpressionKind.As:
                    return OperatorKind.AsOperator;
                case ExpressionKind.Is:
                    return OperatorKind.IsOperator;
            };

            if (kind == ExpressionKind.Plus) {

                if ((leftType.BaseType == BaseType.Char || leftType.BaseType == BaseType.String) &&
                    (rightType.BaseType == BaseType.Char || rightType.BaseType == BaseType.String))
                    return OperatorKind.ConcatOperator;

                if ((leftType.BaseType == BaseType.Integer || leftType.BaseType == BaseType.Real) &&
                    (rightType.BaseType == BaseType.Integer || rightType.BaseType == BaseType.Real))
                    return OperatorKind.PlusOperator;

                if (leftType.BaseType == BaseType.Set && rightType.BaseType == BaseType.Set)
                    return OperatorKind.SetAddOperator;

                if (leftType.BaseType == BaseType.Array && rightType.BaseType == BaseType.Array)
                    return OperatorKind.ConcatArrayOperator;

            }

            if (kind == ExpressionKind.Minus) {

                if ((leftType.BaseType == BaseType.Integer || leftType.BaseType == BaseType.Real) &&
                    (rightType.BaseType == BaseType.Integer || rightType.BaseType == BaseType.Real))
                    return OperatorKind.MinusOperator;

                if (leftType.BaseType == BaseType.Set && rightType.BaseType == BaseType.Set)
                    return OperatorKind.SetDifferenceOperator;

            }

            if (kind == ExpressionKind.Times) {

                if ((leftType.BaseType == BaseType.Integer || leftType.BaseType == BaseType.Real) &&
                    (rightType.BaseType == BaseType.Integer || rightType.BaseType == BaseType.Real))
                    return OperatorKind.TimesOperator;

                if (leftType.BaseType == BaseType.Set && rightType.BaseType == BaseType.Set)
                    return OperatorKind.SetIntersectOperator;

            }

            return OperatorKind.Undefined;
        }

        /// <summary>
        ///     determine the resulting type for a subrange type
        /// </summary>
        /// <param name="types">used type registry</param>
        /// <param name="typeCreator"></param>
        /// <param name="lowerBound">lower bound of the subrange</param>
        /// <param name="upperBound">upper bound of the subrange type</param>
        /// <returns></returns>
        public static ITypeDefinition GetTypeForSubrangeType(this ITypeRegistry types, ITypeCreator typeCreator, ITypeSymbol lowerBound, ITypeSymbol upperBound) {
            var left = lowerBound.TypeDefinition.BaseType;
            var right = upperBound.TypeDefinition.BaseType;

            if (!lowerBound.IsConstant() || !upperBound.IsConstant())
                return types.SystemUnit.ErrorType;

            var isIntegral = left == BaseType.Integer && right == BaseType.Integer;
            var isChar = left == BaseType.Char && right == BaseType.Char;
            var isBoolean = left == BaseType.Boolean && right == BaseType.Boolean;
            var isEnum = left == BaseType.Enumeration && right == BaseType.Enumeration;

            if (isIntegral || isChar || isBoolean || isEnum) {

                if (types.Runtime.IsValueGreaterThen(lowerBound, upperBound)) // lower bound > upper bound?
                    return types.SystemUnit.ErrorType;

                var bothUnsinged = types.Runtime.AreValuesUnsigned(lowerBound, upperBound);
                var baseType = types.SystemUnit.ErrorType as ITypeDefinition;

                if (isIntegral)
                    baseType = types.GetSmallestIntegralTypeOrNext(lowerBound.TypeDefinition, upperBound.TypeDefinition, 1, bothUnsinged);
                else if (isChar)
                    baseType = types.GetSmallestCharTypeOrNext(lowerBound.TypeDefinition, upperBound.TypeDefinition);
                else if (isBoolean)
                    baseType = types.GetSmallestBooleanTypeOrNext(lowerBound.TypeDefinition, upperBound.TypeDefinition);
                else if (isEnum && lowerBound.TypeDefinition.Equals(upperBound.TypeDefinition))
                    baseType = lowerBound.TypeDefinition;

                if (baseType.IsErrorType())
                    return baseType;

                var typeDef = typeCreator.CreateSubrangeType(string.Empty, baseType as IOrdinalType, lowerBound as IValue, upperBound as IValue);
                return typeDef;
            }

            if (lowerBound.TypeDefinition.Equals(upperBound.TypeDefinition)) {
                var baseType = upperBound.TypeDefinition as IOrdinalType;
                var typeDef = typeCreator.CreateSubrangeType(string.Empty, baseType, lowerBound as IValue, upperBound as IValue);
                return typeDef;
            }

            return types.SystemUnit.ErrorType;
        }

        /// <summary>
        ///     determine a base type for an array or set type
        /// </summary>
        /// <param name="types"></param>
        /// <param name="baseType"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static ITypeDefinition GetBaseTypeForArrayOrSet(this ITypeRegistry types, ITypeSymbol baseType, ITypeSymbol elementType) {
            if (elementType == default)
                return types.SystemUnit.ErrorType;
            else if (baseType == default)
                return elementType.TypeDefinition;
            else if (baseType.TypeDefinition.BaseType == BaseType.Integer && elementType.TypeDefinition.BaseType == BaseType.Integer)
                return types.GetSmallestIntegralTypeOrNext(baseType.TypeDefinition, elementType.TypeDefinition);
            else if ((baseType.TypeDefinition.BaseType == BaseType.Char || baseType.TypeDefinition.BaseType == BaseType.String) &&
                (elementType.TypeDefinition.BaseType == BaseType.Char || elementType.TypeDefinition.BaseType == BaseType.String))
                return types.GetSmallestTextTypeOrNext(baseType.TypeDefinition, elementType.TypeDefinition);
            else if (baseType.TypeDefinition is IOrdinalType && baseType.Equals(elementType))
                return elementType.TypeDefinition;
            else if (baseType.TypeDefinition.BaseType == BaseType.Real && elementType.TypeDefinition.BaseType == BaseType.Real)
                return types.SystemUnit.ExtendedType;
            else if (baseType.TypeDefinition is IStructuredType structType && structType.StructTypeKind == StructuredTypeKind.Record && types.AreRecordTypesCompatible(baseType.TypeDefinition, elementType.TypeDefinition))
                return elementType.TypeDefinition;

            return types.SystemUnit.ErrorType;
        }

        /// <summary>
        ///     generate a new set type definition
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="types"></param>
        /// <param name="typeCreator"></param>
        /// <returns></returns>
        public static ITypeDefinition GetMatchingSetType(this ITypeRegistry types, ITypeCreator typeCreator, ITypeDefinition left, ITypeDefinition right) {
            var baseType = types.GetMatchingSetBaseType(left, right, out var newType);
            if (baseType.IsErrorType())
                return baseType;

            if (!newType) {
                return left;
            }

            return typeCreator.CreateSetType(baseType as IOrdinalType, string.Empty);
        }

        /// <summary>
        ///     check if the sets have a compatible base type
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool HaveSetsCommonBaseType(this ITypeRegistry typeRegistry, ITypeSymbol left, ITypeSymbol right) {
            var baseType = typeRegistry.GetMatchingSetBaseType(left.TypeDefinition, right.TypeDefinition, out var _);
            return baseType.BaseType != BaseType.Error;
        }

        /// <summary>
        ///     find a matching set base type
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="typeRegistry"></param>
        /// <param name="requireNewType"></param>
        /// <returns></returns>
        public static ITypeDefinition GetMatchingSetBaseType(this ITypeRegistry typeRegistry, ITypeDefinition left, ITypeDefinition right, out bool requireNewType) {
            requireNewType = false;

            if (!(left is ISetType leftType))
                return typeRegistry.SystemUnit.ErrorType;

            if (!(right is ISetType rightType))
                return typeRegistry.SystemUnit.ErrorType;

            if (!(leftType.BaseTypeDefinition.ResolveAlias() is IOrdinalType leftBaseType))
                return typeRegistry.SystemUnit.ErrorType;

            if (!(rightType.BaseTypeDefinition.ResolveAlias() is IOrdinalType rightBaseType))
                return typeRegistry.SystemUnit.ErrorType;

            if (leftBaseType.Equals(rightBaseType))
                return leftBaseType;

            if (leftBaseType is ISubrangeType lsrt && lsrt.SubrangeOfType.ResolveAlias() is IOrdinalType lsrtb)
                leftBaseType = lsrtb;

            if (rightBaseType is ISubrangeType rsrt && rsrt.SubrangeOfType.ResolveAlias() is IOrdinalType rsrtb)
                leftBaseType = rsrtb;

            if (leftBaseType.Equals(rightBaseType))
                return leftBaseType;

            if (leftBaseType is IIntegralType && rightBaseType is IIntegralType) {
                var result = typeRegistry.GetSmallestIntegralTypeOrNext(leftBaseType, rightBaseType);
                requireNewType = !result.Equals(leftBaseType) || !result.Equals(rightBaseType);
                return result;
            }

            if (leftBaseType is ICharType && rightBaseType is ICharType) {
                var result = typeRegistry.GetSmallestCharTypeOrNext(leftBaseType, rightBaseType);
                requireNewType = !result.Equals(leftBaseType) || !result.Equals(rightBaseType); return result;
            }
            if (leftBaseType is IBooleanType && rightBaseType is IBooleanType) {
                var result = typeRegistry.GetSmallestBooleanTypeOrNext(leftBaseType, rightBaseType);
                requireNewType = !result.Equals(leftBaseType) || !result.Equals(rightBaseType);
                return result;
            }

            return typeRegistry.SystemUnit.ErrorType;
        }

        /// <summary>
        ///     get the size of a generic pointer
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <returns></returns>
        public static uint GetPointerSize(this ITypeRegistry typeRegistry)
            => typeRegistry.SystemUnit.GenericPointerType.TypeSizeInBytes;

        /// <summary>
        ///     check if two types have a common base class
        /// </summary>
        /// <param name="typeRegistry"></param>
        /// <param name="leftTypeId"></param>
        /// <param name="rightTypeId"></param>
        /// <returns></returns>
        public static bool AreCommonBaseClasses(this ITypeRegistry typeRegistry, ITypeDefinition leftTypeId, ITypeDefinition rightTypeId) {
            var leftClass = leftTypeId as IStructuredType;
            var rightClass = rightTypeId as IStructuredType;

            if (leftClass.Equals(rightClass as INamedTypeSymbol))
                return true;

            var baseClass = leftClass.BaseClass as IStructuredType;
            while (baseClass != default) {
                if (baseClass.Equals(rightClass as INamedTypeSymbol))
                    return true;
                baseClass = baseClass.BaseClass as IStructuredType;
            }

            baseClass = rightClass.BaseClass as IStructuredType;
            while (baseClass != default) {
                if (baseClass.Equals(leftClass as INamedTypeSymbol))
                    return true;
                baseClass = baseClass.BaseClass as IStructuredType;
            }

            return false;
        }

    }
}
