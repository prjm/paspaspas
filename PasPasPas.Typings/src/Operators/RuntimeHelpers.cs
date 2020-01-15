using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     helper class
    /// </summary>
    public static class RuntimeHelpers {

        /// <summary>
        ///     simple helper: get arithmetic operations for an unary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="types"></param>
        /// <param name="type">type reference</param>
        /// <returns></returns>
        public static IArithmeticOperations GetArithmeticOperators(this IRuntimeValueFactory runtime, ITypeRegistry types, IOldTypeReference type) {
            var typeKind = type.TypeKind;

            if (typeKind.IsIntegral())
                return runtime.Integers;

            if (typeKind == CommonTypeKind.RealType)
                return runtime.RealNumbers;

            if (types.IsSubrangeType(type.TypeId, out var subrangeType)) {
                var baseType = subrangeType.BaseType;
                var typeRef = runtime.Types.MakeTypeInstanceReference(baseType.TypeId, baseType.TypeKind);
                return runtime.GetArithmeticOperators(types, typeRef);
            }

            return null;
        }

        /// <summary>
        ///     simple helper: get logical operations for an unary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="types">type registry</param>
        /// <param name="type">operand type</param>
        /// <returns></returns>
        public static ILogicalOperations GetLogicalOperators(this IRuntimeValueFactory runtime, ITypeRegistry types, IOldTypeReference type) {
            var typeKind = type.TypeKind;

            if (typeKind == CommonTypeKind.BooleanType)
                return runtime.Booleans;

            if (typeKind.IsIntegral())
                return runtime.Integers;

            if (types.IsSubrangeType(type.TypeId, out var subrangeType)) {
                var baseType = subrangeType.BaseType;
                var typeRef = runtime.Types.MakeTypeInstanceReference(baseType.TypeId, baseType.TypeKind);
                return runtime.GetLogicalOperators(types, typeRef);
            }

            return null;
        }

        /// <summary>
        ///     simple helper get arithmetic operation for a binary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="types"></param>
        /// <param name="leftType">left operand</param>
        /// <param name="rightType">right operand</param>
        /// <returns></returns>
        public static IArithmeticOperations GetArithmeticOperators(this IRuntimeValueFactory runtime, ITypeRegistry types, IOldTypeReference leftType, IOldTypeReference rightType) {
            var left = leftType.TypeKind;
            var right = rightType.TypeKind;

            if (left == CommonTypeKind.RealType && right.IsNumerical())
                return runtime.RealNumbers;

            if (right == CommonTypeKind.RealType && left.IsNumerical())
                return runtime.RealNumbers;

            if (right.IsNumerical() && left.IsNumerical())
                return runtime.Integers;

            if (types.IsSubrangeType(leftType.TypeId, out var subrangeType1)) {
                var baseType = subrangeType1.BaseType;
                var typeRef = runtime.Types.MakeTypeInstanceReference(baseType.TypeId, baseType.TypeKind);
                return GetArithmeticOperators(runtime, types, typeRef, rightType);
            }

            if (types.IsSubrangeType(rightType.TypeId, out var subrangeType2)) {
                var baseType = subrangeType2.BaseType;
                var typeRef = runtime.Types.MakeTypeInstanceReference(baseType.TypeId, baseType.TypeKind);
                return GetArithmeticOperators(runtime, types, leftType, typeRef);
            }

            return null;
        }

        /// <summary>
        ///     simple helper: get logical operations for a binary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="leftType">left operand</param>
        /// <param name="rightType">right operand</param>
        /// <param name="types">type registry</param>
        /// <returns></returns>
        public static ILogicalOperations GetLogicalOperators(this IRuntimeValueFactory runtime, ITypeRegistry types, IOldTypeReference leftType, IOldTypeReference rightType) {
            var left = leftType.TypeKind;
            var right = rightType.TypeKind;

            if (left == CommonTypeKind.BooleanType && right == CommonTypeKind.BooleanType)
                return runtime.Booleans;

            if (left.IsIntegral() && right.IsIntegral())
                return runtime.Integers;

            if (types.IsSubrangeType(leftType.TypeId, out var subrangeType1)) {
                var baseType = subrangeType1.BaseType;
                var typeRef = runtime.Types.MakeTypeInstanceReference(baseType.TypeId, baseType.TypeKind);
                return GetLogicalOperators(runtime, types, typeRef, rightType);
            }

            if (types.IsSubrangeType(rightType.TypeId, out var subrangeType2)) {
                var baseType = subrangeType2.BaseType;
                var typeRef = runtime.Types.MakeTypeInstanceReference(baseType.TypeId, baseType.TypeKind);
                return GetLogicalOperators(runtime, types, leftType, typeRef);
            }

            return default;
        }

        /// <summary>
        ///     simple helper: get relational operations for a binary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="types"></param>
        /// <param name="leftType">left operand</param>
        /// <param name="rightType">right operand</param>
        /// <returns></returns>
        public static IRelationalOperations GetRelationalOperators(this IRuntimeValueFactory runtime, ITypeRegistry types, IOldTypeReference leftType, IOldTypeReference rightType) {
            var left = leftType.TypeKind;
            var right = rightType.TypeKind;

            if (left == CommonTypeKind.RealType && right.IsNumerical())
                return runtime.RealNumbers;

            if (right == CommonTypeKind.RealType && left.IsNumerical())
                return runtime.RealNumbers;

            if (right.IsNumerical() && left.IsNumerical())
                return runtime.Integers;

            if (left == CommonTypeKind.BooleanType && right == CommonTypeKind.BooleanType)
                return runtime.Booleans;

            if (left.IsTextual() && right.IsTextual())
                return runtime.Strings;

            if (types.IsSubrangeType(leftType.TypeId, out var subrangeType1)) {
                var baseType = subrangeType1.BaseType;
                var typeRef = runtime.Types.MakeTypeInstanceReference(baseType.TypeId, baseType.TypeKind);
                return GetRelationalOperators(runtime, types, typeRef, rightType);
            }

            if (types.IsSubrangeType(rightType.TypeId, out var subrangeType2)) {
                var baseType = subrangeType2.BaseType;
                var typeRef = runtime.Types.MakeTypeInstanceReference(baseType.TypeId, baseType.TypeKind);
                return GetRelationalOperators(runtime, types, leftType, typeRef);
            }

            if (right.IsSet() && left.IsSet())
                return runtime.Structured;

            return default;
        }

        /// <summary>
        ///     simple helper: get string operations for a binary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <returns></returns>
        public static IStringOperations GetStringOperators(this IRuntimeValueFactory runtime, CommonTypeKind left, CommonTypeKind right) {

            if (left.IsTextual() && right.IsTextual())
                return runtime.Strings;

            return default;
        }

        /// <summary>
        ///     helper function: check if one value is greater then another
        /// </summary>
        /// <param name="runtime">used runtime</param>
        /// <param name="left">first operand</param>
        /// <param name="right">second operand</param>
        /// <returns><c>true</c> if the first operand is greater then the second operand</returns>
        public static bool IsValueGreaterThen(this IRuntimeValueFactory runtime, IOldTypeReference left, IOldTypeReference right) {

            if (!left.IsConstant())
                return false;

            if (!right.IsConstant())
                return false;

            if (left is IIntegerValue && right is IIntegerValue) {

                if (!(runtime.Integers.GreaterThen(left, right) is IBooleanValue result))
                    return false;

                return result.AsBoolean;
            }

            if (left is ICharValue && right is ICharValue) {

                if (!(runtime.Strings.GreaterThen(left, right) is IBooleanValue result))
                    return false;

                return result.AsBoolean;
            }

            if (left is IBooleanValue && right is IBooleanValue) {

                if (!(runtime.Booleans.GreaterThen(left, right) is IBooleanValue result))
                    return false;

                return result.AsBoolean;
            }

            if (left is IEnumeratedValue leftEnum && right is IEnumeratedValue rightEnum) {

                if (!(runtime.Integers.GreaterThen(leftEnum.Value, rightEnum.Value) is IBooleanValue result))
                    return false;

                return result.AsBoolean;


            }

            return false;
        }


        /// <summary>
        ///     test if two values are unsigned
        /// </summary>
        /// <param name="runtime">used runtime</param>
        /// <param name="left">first operand</param>
        /// <param name="right">second operand</param>
        /// <returns></returns>
        public static bool AreValuesUnsigned(this IRuntimeValueFactory runtime, IOldTypeReference left, IOldTypeReference right) {
            if (!left.IsConstant())
                return false;

            if (!right.IsConstant())
                return false;

            if (!(left is IIntegerValue leftInt))
                return false;

            if (!(right is IIntegerValue rightInt))
                return false;

            return !leftInt.IsNegative && !rightInt.IsNegative;
        }

    }
}
