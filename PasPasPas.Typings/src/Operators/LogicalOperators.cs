using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     logical operators: type deduction and constant propagation
    /// </summary>
    public class LogicalOperator : OperatorBase {

        private static void Register(ITypeRegistry registry, int kind, int arity = 2)
            => registry.RegisterOperator(new LogicalOperator(kind, arity));

        /// <summary>
        ///     register logical operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, DefinedOperators.NotOperator, 1);
            Register(registry, DefinedOperators.AndOperator);
            Register(registry, DefinedOperators.XorOperator);
            Register(registry, DefinedOperators.OrOperator);
            Register(registry, DefinedOperators.ShlOperator);
            Register(registry, DefinedOperators.ShrOperator);
        }

        /// <summary>
        ///     create a new logical operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">operator arity</param>
        public LogicalOperator(int withKind, int withArity)
            : base(withKind, withArity) { }

        /// <summary>
        ///     operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.AndOperator:
                        return "and";
                    case DefinedOperators.OrOperator:
                        return "or";
                    case DefinedOperators.XorOperator:
                        return "xor";
                    case DefinedOperators.NotOperator:
                        return "not";
                    case DefinedOperators.ShlOperator:
                        return "shl";
                    case DefinedOperators.ShrOperator:
                        return "shr";
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     evaluate an unary operator
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        protected override ITypeSymbol EvaluateUnaryOperator(Signature input) {
            var operand = input[0];
            var operations = Runtime.GetLogicalOperators(TypeRegistry, operand);

            if (operations == null)
                return Invalid;

            if (Kind == DefinedOperators.NotOperator)
                if (operand.IsConstant())
                    return operations.NotOperator(operand);
                else
                    return operand;

            return Invalid;
        }

        /// <summary>
        ///     binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override ITypeSymbol EvaluateBinaryOperator(Signature input) {
            var left = input[0];
            var right = input[1];

            if (Kind == DefinedOperators.ShrOperator)
                return EvaluateShiftOperator(toLeft: false, left, right);

            if (Kind == DefinedOperators.ShlOperator)
                return EvaluateShiftOperator(toLeft: true, left, right);

            var operations = Runtime.GetLogicalOperators(TypeRegistry, left, right);

            if (operations == null)
                return Invalid;

            if (Kind == DefinedOperators.AndOperator)
                return EvaluateAndOperator(left, right, operations);

            if (Kind == DefinedOperators.OrOperator)
                return EvaluateOrOperator(left, right, operations);

            if (Kind == DefinedOperators.XorOperator)
                return EvaluateXorOperator(left, right, operations);

            return Invalid;
        }

        private ITypeSymbol EvaluateShiftOperator(bool toLeft, ITypeSymbol left, ITypeSymbol right) {
            if (left.IsConstant() && right.IsConstant())
                if (toLeft)
                    return Runtime.Integers.Shl(left, right);
                else
                    return Runtime.Integers.Shr(left, right);

            var baseType = left.TypeDefinition;

            if (TypeRegistry.IsSubrangeType(left.TypeDefinition, out var subrangeType))
                baseType = subrangeType.SubrangeOfType;

            if (!(baseType is IIntegralType intType))
                return Invalid;

            if (right.IsConstant()) {
                if (right is IIntegerValue intValue && intValue.SignedValue >= 0) {
                    if (intValue.SignedValue <= 32 && intType.TypeSizeInBytes < 4)
                        return SystemUnit.IntegerType;
                    if (intValue.SignedValue <= 32 && intType.TypeSizeInBytes == 4)
                        return baseType;
                    if (intType.TypeSizeInBytes == 8)
                        return baseType;
                }

                return Invalid;
            }

            if (intType.TypeSizeInBytes < 4)
                return SystemUnit.IntegerType;

            return intType;
        }

        private ITypeSymbol EvaluateXorOperator(ITypeSymbol left, ITypeSymbol right, ILogicalOperations operations) {
            if (left.IsConstant() && right.IsConstant())
                return operations.XorOperator(left, right);
            else
                return GetSmallestBoolOrIntegralType(left, right, 1);
        }

        private ITypeSymbol EvaluateOrOperator(ITypeSymbol left, ITypeSymbol right, ILogicalOperations operations) {
            if (left.IsConstant() && right.IsConstant())
                return operations.OrOperator(left, right);
            else if (left.IsConstant() && Runtime.Booleans.TrueValue.Equals(left))
                return Runtime.Booleans.TrueValue;
            else if (left.IsConstant() && Runtime.Booleans.FalseValue.Equals(false))
                return right;
            else
                return GetSmallestBoolOrIntegralType(left, right, 1);
        }

        private ITypeSymbol EvaluateAndOperator(ITypeSymbol left, ITypeSymbol right, ILogicalOperations operations) {
            if (left.IsConstant() && right.IsConstant())
                return operations.AndOperator(left, right);
            else if (left.IsConstant() && Runtime.Booleans.FalseValue.Equals(left))
                return Runtime.Booleans.FalseValue;
            else if (left.IsConstant() && Runtime.Booleans.TrueValue.Equals(left))
                return right;
            else
                return GetSmallestBoolOrIntegralType(left, right, 1);
        }
    }
}