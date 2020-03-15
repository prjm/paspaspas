using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     provides arithmetic operators: type deduction and constant propagation
    /// </summary>
    public class ArithmeticOperator : OperatorBase {

        /// <summary>
        ///     helper function: register an operator
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="kind">operator kind</param>
        /// <param name="arity">operator arity</param>
        private static void Register(ITypeRegistry registry, int kind, int arity = 2)
            => registry.RegisterOperator(new ArithmeticOperator(kind, arity));

        /// <summary>
        ///     register known operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, DefinedOperators.UnaryMinus, 1);
            Register(registry, DefinedOperators.UnaryPlus, 1);
            Register(registry, DefinedOperators.PlusOperator);
            Register(registry, DefinedOperators.MinusOperator);
            Register(registry, DefinedOperators.TimesOperator);
            Register(registry, DefinedOperators.DivOperator);
            Register(registry, DefinedOperators.ModOperator);
            Register(registry, DefinedOperators.SlashOperator);
        }

        /// <summary>
        ///     create a new arithmetic operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">arity</param>
        public ArithmeticOperator(int withKind, int withArity = 2)
            : base(withKind, withArity) { }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.UnaryPlus:
                        return "+";
                    case DefinedOperators.UnaryMinus:
                        return "-";
                    case DefinedOperators.PlusOperator:
                        return "+";
                    case DefinedOperators.MinusOperator:
                        return "-";
                    case DefinedOperators.TimesOperator:
                        return "*";
                    case DefinedOperators.DivOperator:
                        return "div";
                    case DefinedOperators.ModOperator:
                        return "mod";
                    case DefinedOperators.SlashOperator:
                        return "/";
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     evaluate an unary operator
        /// </summary>
        /// <param name="input">operator input</param>
        /// <returns>operator result</returns>
        protected override ITypeSymbol EvaluateUnaryOperator(ISignature input) {
            var operand = input[0];
            var operations = Runtime.GetArithmeticOperators(TypeRegistry, operand);

            if (operations == null)
                return Invalid;

            if (Kind == DefinedOperators.UnaryPlus)
                return EvaluateUnaryOperand(negate: false, operand, operations);

            if (Kind == DefinedOperators.UnaryMinus)
                return EvaluateUnaryOperand(negate: true, operand, operations);

            return Invalid;
        }

        private static ITypeSymbol EvaluateUnaryOperand(bool negate, ITypeSymbol operand, IArithmeticOperations operations) {
            if (operand.IsConstant(out var constantOperand)) {
                if (negate)
                    return operations.Negate(constantOperand);
                else
                    return operand;
            }

            return operand;
        }

        /// <summary>
        ///     evaluate a binary operator
        /// </summary>
        /// <param name="input">input signature</param>
        /// <returns>operator result</returns>
        protected override ITypeSymbol EvaluateBinaryOperator(ISignature input) {
            var left = input[0];
            var right = input[1];

            if (Kind == DefinedOperators.DivOperator)
                return EvaluateDivOperator(left, right);

            if (Kind == DefinedOperators.ModOperator)
                return EvaluateModOperator(left, right);

            if (Kind == DefinedOperators.SlashOperator)
                return EvaluateRealDivOperator(left, right);

            var operations = Runtime.GetArithmeticOperators(TypeRegistry, left, right);

            if (operations == null)
                return Invalid;

            if (Kind == DefinedOperators.PlusOperator)
                return EvaluatePlusOperator(left, right, operations);

            if (Kind == DefinedOperators.MinusOperator)
                return EvaluateMinusOperator(left, right, operations);

            if (Kind == DefinedOperators.TimesOperator)
                return EvaluateMultiplicationOperator(left, right, operations);

            return Invalid;
        }

        private ITypeSymbol EvaluateMultiplicationOperator(ITypeSymbol left, ITypeSymbol right, IArithmeticOperations operations) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return operations.Multiply(l, r);
            else
                return GetSmallestRealOrIntegralType(left, right, 32);
        }

        private ITypeSymbol EvaluateMinusOperator(ITypeSymbol left, ITypeSymbol right, IArithmeticOperations operations) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return operations.Subtract(l, r);
            else
                return GetSmallestRealOrIntegralType(left, right, 32);
        }

        private ITypeSymbol EvaluatePlusOperator(ITypeSymbol left, ITypeSymbol right, IArithmeticOperations operations) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return operations.Add(l, r);
            else
                return GetSmallestRealOrIntegralType(left, right, 32);
        }


        private ITypeSymbol EvaluateRealDivOperator(ITypeSymbol left, ITypeSymbol right) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return Runtime.RealNumbers.Divide(l, r);
            else
                return ExtendedType;
        }

        private ITypeSymbol EvaluateModOperator(ITypeSymbol left, ITypeSymbol right) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return Runtime.Integers.Modulo(l, r);
            else
                return GetSmallestIntegralType(left, right, 32);
        }

        private ITypeSymbol EvaluateDivOperator(ITypeSymbol left, ITypeSymbol right) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return Runtime.Integers.Divide(l, r);
            else
                return GetSmallestIntegralType(left, right, 32);
        }
    }
}