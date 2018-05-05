using PasPasPas.Global.Runtime;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     helper class
    /// </summary>
    public static class RuntimeHelpers {

        private static ITypeReference GetBaseTypeOfSubrangeType(this IRuntimeValueFactory runtime, int typeId)
            => runtime.Types.MakeReference(runtime.Types.Resolver.GetBaseTypeOfSubrangeType(typeId));

        /// <summary>
        ///     simple helper: get arithmetic operations for an unary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="type">type reference</param>
        /// <returns></returns>
        public static IArithmeticOperations GetArithmeticOperators(this IRuntimeValueFactory runtime, ITypeReference type) {
            var typeKind = type.TypeKind;

            if (typeKind.IsIntegral())
                return runtime.Integers;

            if (typeKind == CommonTypeKind.RealType)
                return runtime.RealNumbers;

            if (typeKind == CommonTypeKind.SubrangeType)
                return runtime.GetArithmeticOperators(runtime.GetBaseTypeOfSubrangeType(type.TypeId));

            return null;
        }

        /// <summary>
        ///     simple helper: get logical operations for an unary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="type">operand type</param>
        /// <returns></returns>
        public static ILogicalOperations GetLogicalOperators(this IRuntimeValueFactory runtime, ITypeReference type) {
            var typeKind = type.TypeKind;

            if (typeKind == CommonTypeKind.BooleanType)
                return runtime.Booleans;

            if (typeKind.IsIntegral())
                return runtime.Integers;

            if (typeKind == CommonTypeKind.SubrangeType)
                return runtime.GetLogicalOperators(runtime.GetBaseTypeOfSubrangeType(type.TypeId));

            return null;
        }

        /// <summary>
        ///     simple helper get arithmetic operation for a binary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="leftType">left operand</param>
        /// <param name="rightType">right operand</param>
        /// <returns></returns>
        public static IArithmeticOperations GetArithmeticOperators(this IRuntimeValueFactory runtime, ITypeReference leftType, ITypeReference rightType) {
            var left = leftType.TypeKind;
            var right = rightType.TypeKind;

            if (left == CommonTypeKind.RealType && right.IsNumerical())
                return runtime.RealNumbers;

            if (right == CommonTypeKind.RealType && left.IsNumerical())
                return runtime.RealNumbers;

            if (right.IsNumerical() && left.IsNumerical())
                return runtime.Integers;

            if (left == CommonTypeKind.SubrangeType)
                return GetArithmeticOperators(runtime, runtime.GetBaseTypeOfSubrangeType(leftType.TypeId), rightType);

            if (right == CommonTypeKind.SubrangeType)
                return GetArithmeticOperators(runtime, leftType, runtime.GetBaseTypeOfSubrangeType(rightType.TypeId));

            return null;
        }

        /// <summary>
        ///     simple helper: get logical operations for a binary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="leftType">left operand</param>
        /// <param name="rightType">right operand</param>
        /// <returns></returns>
        public static ILogicalOperations GetLogicalOperators(this IRuntimeValueFactory runtime, ITypeReference leftType, ITypeReference rightType) {
            var left = leftType.TypeKind;
            var right = rightType.TypeKind;

            if (left == CommonTypeKind.BooleanType && right == CommonTypeKind.BooleanType)
                return runtime.Booleans;

            if (left.IsIntegral() && right.IsIntegral())
                return runtime.Integers;

            if (left == CommonTypeKind.SubrangeType)
                return GetLogicalOperators(runtime, runtime.GetBaseTypeOfSubrangeType(leftType.TypeId), rightType);

            if (right == CommonTypeKind.SubrangeType)
                return GetLogicalOperators(runtime, leftType, runtime.GetBaseTypeOfSubrangeType(rightType.TypeId));


            return default;
        }

        /// <summary>
        ///     simple helper: get relational operations for a binary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="leftType">left operand</param>
        /// <param name="rightType">right operand</param>
        /// <returns></returns>
        public static IRelationalOperations GetRelationalOperators(this IRuntimeValueFactory runtime, ITypeReference leftType, ITypeReference rightType) {
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
        public static bool IsValueGreaterThen(this IRuntimeValueFactory runtime, ITypeReference left, ITypeReference right) {

            if (!left.IsConstant)
                return false;

            if (!right.IsConstant)
                return false;

            if (left is IIntegerValue && right is IIntegerValue) {
                var result = runtime.Integers.GreaterThen(left, right) as IBooleanValue;

                if (result == null)
                    return false;

                return result.AsBoolean;
            }

            if (left is ICharValue && right is ICharValue) {
                var result = runtime.Strings.GreaterThen(left, right) as IBooleanValue;

                if (result == null)
                    return false;

                return result.AsBoolean;
            }

            if (left is IBooleanValue && right is IBooleanValue) {
                var result = runtime.Booleans.GreaterThen(left, right) as IBooleanValue;

                if (result == null)
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
        public static bool AreValuesUnsigned(this IRuntimeValueFactory runtime, ITypeReference left, ITypeReference right) {
            if (!left.IsConstant)
                return false;

            if (!right.IsConstant)
                return false;

            if (!(left is IIntegerValue leftInt))
                return false;

            if (!(right is IIntegerValue rightInt))
                return false;

            return !leftInt.IsNegative && !rightInt.IsNegative;
        }

    }
}
