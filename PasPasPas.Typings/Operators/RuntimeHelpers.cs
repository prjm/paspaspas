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

    }
}
