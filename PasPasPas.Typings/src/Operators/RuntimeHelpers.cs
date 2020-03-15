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
        public static IArithmeticOperations GetArithmeticOperators(this IRuntimeValueFactory runtime, ITypeRegistry types, ITypeSymbol type) {
            var typeKind = type.TypeDefinition.BaseType;

            if (typeKind == BaseType.Integer)
                return runtime.Integers;

            if (typeKind == BaseType.Real)
                return runtime.RealNumbers;

            if (type.TypeDefinition.IsSubrangeType(out var subrangeType)) {
                var baseType = subrangeType.SubrangeOfType;
                return runtime.GetArithmeticOperators(types, baseType);
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
        public static ILogicalOperations GetLogicalOperators(this IRuntimeValueFactory runtime, ITypeRegistry types, ITypeSymbol type) {
            var typeKind = type.TypeDefinition.BaseType;

            if (typeKind == BaseType.Boolean)
                return runtime.Booleans;

            if (typeKind == BaseType.Integer)
                return runtime.Integers;

            if (type.TypeDefinition.IsSubrangeType(out var subrangeType)) {
                var baseType = subrangeType.SubrangeOfType;
                return runtime.GetLogicalOperators(types, baseType);
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
        public static IArithmeticOperations GetArithmeticOperators(this IRuntimeValueFactory runtime, ITypeRegistry types, ITypeSymbol leftType, ITypeSymbol rightType) {
            var left = leftType.TypeDefinition.BaseType;
            var right = rightType.TypeDefinition.BaseType;

            if (left == BaseType.Real && (right == BaseType.Real || right == BaseType.Integer))
                return runtime.RealNumbers;

            if (right == BaseType.Real && (left == BaseType.Real || left == BaseType.Integer))
                return runtime.RealNumbers;

            if (right == BaseType.Integer && left == BaseType.Integer)
                return runtime.Integers;

            if (leftType.TypeDefinition.IsSubrangeType(out var subrangeType1)) {
                var baseType = subrangeType1.SubrangeOfType;
                return GetArithmeticOperators(runtime, types, baseType, rightType);
            }

            if (rightType.TypeDefinition.IsSubrangeType(out var subrangeType2)) {
                var baseType = subrangeType2.SubrangeOfType;
                return GetArithmeticOperators(runtime, types, leftType, baseType);
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
        public static ILogicalOperations GetLogicalOperators(this IRuntimeValueFactory runtime, ITypeRegistry types, ITypeSymbol leftType, ITypeSymbol rightType) {
            var left = leftType.TypeDefinition.BaseType;
            var right = rightType.TypeDefinition.BaseType;

            if (left == BaseType.Boolean && right == BaseType.Boolean)
                return runtime.Booleans;

            if (left == BaseType.Integer && right == BaseType.Integer)
                return runtime.Integers;

            if (leftType.TypeDefinition.IsSubrangeType(out var subrangeType1)) {
                var baseType = subrangeType1.SubrangeOfType;
                return GetLogicalOperators(runtime, types, baseType, rightType);
            }

            if (leftType.TypeDefinition.IsSubrangeType(out var subrangeType2)) {
                var baseType = subrangeType2.SubrangeOfType;
                return GetLogicalOperators(runtime, types, leftType, baseType);
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
        public static IRelationalOperations GetRelationalOperators(this IRuntimeValueFactory runtime, ITypeRegistry types, ITypeSymbol leftType, ITypeSymbol rightType) {
            var left = leftType.TypeDefinition.BaseType;
            var right = rightType.TypeDefinition.BaseType;

            if (left == BaseType.Real && (right == BaseType.Real || right == BaseType.Integer))
                return runtime.RealNumbers;

            if (right == BaseType.Real && (left == BaseType.Real || left == BaseType.Integer))
                return runtime.RealNumbers;

            if (right == BaseType.Integer && left == BaseType.Integer)
                return runtime.Integers;

            if (left == BaseType.Boolean && right == BaseType.Boolean)
                return runtime.Booleans;

            if ((left == BaseType.Char || left == BaseType.String) && (right == BaseType.Char || right == BaseType.String))
                return runtime.Strings;

            if (leftType.TypeDefinition.IsSubrangeType(out var subrangeType1)) {
                var baseType = subrangeType1.SubrangeOfType;
                return GetRelationalOperators(runtime, types, baseType, rightType);
            }

            if (rightType.TypeDefinition.IsSubrangeType(out var subrangeType2)) {
                var baseType = subrangeType2.SubrangeOfType;
                return GetRelationalOperators(runtime, types, leftType, baseType);
            }

            if (right == BaseType.Set && left == BaseType.Set)
                return runtime.Structured;

            return default;
        }

        /// <summary>
        ///     simple helper: get string operations for a binary operator
        /// </summary>
        /// <param name="runtime">runtime to use</param>
        /// <param name="leftType">left operand</param>
        /// <param name="rightType">right operand</param>
        /// <returns></returns>
        public static IStringOperations GetStringOperators(this IRuntimeValueFactory runtime, ITypeSymbol leftType, ITypeSymbol rightType) {
            var left = leftType.TypeDefinition.BaseType;
            var right = rightType.TypeDefinition.BaseType;

            if ((left == BaseType.Char || left == BaseType.String) && (right == BaseType.Char || right == BaseType.String))
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
        public static bool IsValueGreaterThen(this IRuntimeValueFactory runtime, ITypeSymbol left, ITypeSymbol right) {

            if (!left.IsConstant())
                return false;

            if (!right.IsConstant())
                return false;

            if (left is IIntegerValue ileft && right is IIntegerValue iright) {

                if (!(runtime.Integers.GreaterThen(ileft, iright) is IBooleanValue result))
                    return false;

                return result.AsBoolean;
            }

            if (left is ICharValue cleft && right is ICharValue cright) {

                if (!(runtime.Strings.GreaterThen(cleft, cright) is IBooleanValue result))
                    return false;

                return result.AsBoolean;
            }

            if (left is IBooleanValue bleft && right is IBooleanValue bright) {

                if (!(runtime.Booleans.GreaterThen(bleft, bright) is IBooleanValue result))
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
        public static bool AreValuesUnsigned(this IRuntimeValueFactory runtime, ITypeSymbol left, ITypeSymbol right) {
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
