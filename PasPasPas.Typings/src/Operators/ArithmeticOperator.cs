#nullable disable
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
        private static void Register(ITypeRegistry registry, OperatorKind kind, int arity = 2)
            => registry.RegisterOperator(new ArithmeticOperator(kind, arity));

        /// <summary>
        ///     register known operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, OperatorKind.UnaryMinus, 1);
            Register(registry, OperatorKind.UnaryPlus, 1);
            Register(registry, OperatorKind.PlusOperator);
            Register(registry, OperatorKind.MinusOperator);
            Register(registry, OperatorKind.TimesOperator);
            Register(registry, OperatorKind.DivOperator);
            Register(registry, OperatorKind.ModOperator);
            Register(registry, OperatorKind.SlashOperator);
        }

        /// <summary>
        ///     create a new arithmetic operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">arity</param>
        public ArithmeticOperator(OperatorKind withKind, int withArity = 2)
            : base(withKind, withArity) { }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case OperatorKind.UnaryPlus:
                        return KnownNames.Plus;
                    case OperatorKind.UnaryMinus:
                        return KnownNames.Minus;
                    case OperatorKind.PlusOperator:
                        return KnownNames.Plus;
                    case OperatorKind.MinusOperator:
                        return KnownNames.Minus;
                    case OperatorKind.TimesOperator:
                        return KnownNames.Star;
                    case OperatorKind.DivOperator:
                        return KnownNames.Div;
                    case OperatorKind.ModOperator:
                        return KnownNames.Mod;
                    case OperatorKind.SlashOperator:
                        return KnownNames.Slash;
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     evaluate an unary operator
        /// </summary>
        /// <param name="input">operator input</param>
        /// <param name="currentUnit">current unit</param>
        /// <returns>operator result</returns>
        protected override ITypeSymbol EvaluateUnaryOperator(ISignature input, IUnitType currentUnit) {
            var operand = input[0];
            var operations = Runtime.GetArithmeticOperators(TypeRegistry, operand);

            if (operations == null)
                return Invalid;

            if (Kind == OperatorKind.UnaryPlus)
                return EvaluateUnaryOperand(negate: false, operand, operations);

            if (Kind == OperatorKind.UnaryMinus)
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
        /// <param name="currentUnit">current unit</param>
        /// <returns>operator result</returns>
        protected override ITypeSymbol EvaluateBinaryOperator(ISignature input, IUnitType currentUnit) {
            var left = input[0];
            var right = input[1];

            if (Kind == OperatorKind.DivOperator)
                return EvaluateDivOperator(left, right);

            if (Kind == OperatorKind.ModOperator)
                return EvaluateModOperator(left, right);

            if (Kind == OperatorKind.SlashOperator)
                return EvaluateRealDivOperator(left, right);

            var operations = Runtime.GetArithmeticOperators(TypeRegistry, left, right);

            if (operations == null)
                return Invalid;

            if (Kind == OperatorKind.PlusOperator)
                return EvaluatePlusOperator(left, right, operations);

            if (Kind == OperatorKind.MinusOperator)
                return EvaluateMinusOperator(left, right, operations);

            if (Kind == OperatorKind.TimesOperator)
                return EvaluateMultiplicationOperator(left, right, operations);

            return Invalid;
        }

        private ITypeSymbol EvaluateMultiplicationOperator(ITypeSymbol left, ITypeSymbol right, IArithmeticOperations operations) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return operations.Multiply(l, r);
            else
                return GetSmallestRealOrIntegralType(left, right, 32).Reference;
        }

        private ITypeSymbol EvaluateMinusOperator(ITypeSymbol left, ITypeSymbol right, IArithmeticOperations operations) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return operations.Subtract(l, r);
            else
                return GetSmallestRealOrIntegralType(left, right, 32).Reference;
        }

        private ITypeSymbol EvaluatePlusOperator(ITypeSymbol left, ITypeSymbol right, IArithmeticOperations operations) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return operations.Add(l, r);
            else
                return GetSmallestRealOrIntegralType(left, right, 32).Reference;
        }


        private ITypeSymbol EvaluateRealDivOperator(ITypeSymbol left, ITypeSymbol right) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return Runtime.RealNumbers.Divide(l, r);
            else
                return ExtendedType.Reference;
        }

        private ITypeSymbol EvaluateModOperator(ITypeSymbol left, ITypeSymbol right) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return Runtime.Integers.Modulo(l, r);
            else
                return GetSmallestIntegralType(left, right, 32).Reference;
        }

        private ITypeSymbol EvaluateDivOperator(ITypeSymbol left, ITypeSymbol right) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return Runtime.Integers.Divide(l, r);
            else
                return GetSmallestIntegralType(left, right, 32).Reference;
        }
    }
}