using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
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
        /// <param name="typeKind">type kind of the arithmetics interface</param>
        /// <returns></returns>
        public static IArithmeticOperations GetArithmeticOperators(this IRuntimeValueFactory runtime, CommonTypeKind typeKind) {

            if (typeKind.IsIntegral())
                return runtime.Integers;

            if (typeKind == CommonTypeKind.FloatType)
                return runtime.RealNumbers;

            return null;
        }

        /// <summary>
        ///     simple helper: get logical operations for an unary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="typeKind">type kind of the logical operations interface</param>
        /// <returns></returns>
        public static ILogicalOperations GetLogicalOperators(this IRuntimeValueFactory runtime, CommonTypeKind typeKind) {

            if (typeKind == CommonTypeKind.BooleanType)
                return runtime.Booleans;

            if (typeKind.IsIntegral())
                return runtime.Integers;

            return null;
        }

        /// <summary>
        ///     simple helper get arithmetic operation for a binary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <returns></returns>
        public static IArithmeticOperations GetArithmeticOperators(this IRuntimeValueFactory runtime, CommonTypeKind left, CommonTypeKind right) {

            if (left == CommonTypeKind.FloatType && right.IsNumerical())
                return runtime.RealNumbers;

            if (right == CommonTypeKind.FloatType && left.IsNumerical())
                return runtime.RealNumbers;

            if (right.IsNumerical() && left.IsNumerical())
                return runtime.Integers;

            return null;
        }

        /// <summary>
        ///     simple helper: get logical operations for a binary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <returns></returns>
        public static ILogicalOperations GetLogicalOperators(this IRuntimeValueFactory runtime, CommonTypeKind left, CommonTypeKind right) {

            if (left == CommonTypeKind.BooleanType && right == CommonTypeKind.BooleanType)
                return runtime.Booleans;

            if (left.IsIntegral() && right.IsIntegral())
                return runtime.Integers;

            return null;
        }

        /// <summary>
        ///     simple helper: get relational operations for a binary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="left">left operand</param>
        /// <param name="right">right operand</param>
        /// <returns></returns>
        public static IRelationalOperations GetRelationalOperators(this IRuntimeValueFactory runtime, CommonTypeKind left, CommonTypeKind right) {

            if (left == CommonTypeKind.FloatType && right.IsNumerical())
                return runtime.RealNumbers;

            if (right == CommonTypeKind.FloatType && left.IsNumerical())
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
