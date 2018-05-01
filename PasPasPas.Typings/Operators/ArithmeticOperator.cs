using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;

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
        protected override ITypeReference EvaluateUnaryOperator(Signature input) {
            var operand = input[0];
            var operations = Runtime.GetArithmeticOperators(operand);

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.UnaryPlus)
                return EvaluateUnaryOperand(negate: false, operand, operations);

            if (Kind == DefinedOperators.UnaryMinus)
                return EvaluateUnaryOperand(negate: true, operand, operations);

            return GetErrorTypeReference();
        }

        private ITypeReference EvaluateUnaryOperand(bool negate, ITypeReference operand, IArithmeticOperations operations) {
            if (operand.IsConstant) {
                if (negate)
                    return operations.Negate(operand);
                else
                    return operand;
            }

            if (operand.TypeKind != CommonTypeKind.SubrangeType)
                return operand;

            var baseType = TypeRegistry.GetTypeByIdOrUndefinedType(TypeRegistry.GetBaseTypeOfSubrangeType(operand.TypeId)) as IIntegralType;

            if (baseType != null && baseType.BitSize < 32)
                return Runtime.Types.MakeReference(KnownTypeIds.IntegerType);

            if (baseType != null)
                return Runtime.Types.MakeReference(baseType.TypeId);

            return GetErrorTypeReference();
        }

        /// <summary>
        ///     evaluate a binary operator
        /// </summary>
        /// <param name="input">input signature</param>
        /// <returns>operator result</returns>
        protected override ITypeReference EvaluateBinaryOperator(Signature input) {
            var left = input[0];
            var right = input[1];

            if (Kind == DefinedOperators.DivOperator)
                return EvaluateDivOperator(left, right);

            if (Kind == DefinedOperators.ModOperator)
                return EvaluateModOperator(left, right);

            if (Kind == DefinedOperators.SlashOperator)
                return EvaluateRealDivOperator(left, right);

            var operations = Runtime.GetArithmeticOperators(left, right);

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.PlusOperator)
                return EvaluatePlusOperator(left, right, operations);

            if (Kind == DefinedOperators.MinusOperator)
                return EvaluateMinusOperator(left, right, operations);

            if (Kind == DefinedOperators.TimesOperator)
                return EvaluateMultiplicationOperator(left, right, operations);

            return GetErrorTypeReference();
        }

        private ITypeReference EvaluateMultiplicationOperator(ITypeReference left, ITypeReference right, IArithmeticOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.Multiply(left, right);
            else
                return GetSmallestRealOrIntegralType(left, right, 32);
        }

        private ITypeReference EvaluateMinusOperator(ITypeReference left, ITypeReference right, IArithmeticOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.Subtract(left, right);
            else
                return GetSmallestRealOrIntegralType(left, right, 32);
        }

        private ITypeReference EvaluatePlusOperator(ITypeReference left, ITypeReference right, IArithmeticOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.Add(left, right);
            else
                return GetSmallestRealOrIntegralType(left, right, 32);
        }

        private ITypeReference EvaluateRealDivOperator(ITypeReference left, ITypeReference right) {
            if (left.IsConstant && right.IsConstant)
                return Runtime.RealNumbers.Divide(left, right);
            else
                return GetExtendedType();
        }

        private ITypeReference EvaluateModOperator(ITypeReference left, ITypeReference right) {
            if (left.IsConstant && right.IsConstant)
                return Runtime.Integers.Modulo(left, right);
            else
                return GetSmallestIntegralType(left, right, 32);
        }

        private ITypeReference EvaluateDivOperator(ITypeReference left, ITypeReference right) {
            if (left.IsConstant && right.IsConstant)
                return Runtime.Integers.Divide(left, right);
            else
                return GetSmallestIntegralType(left, right, 32);
        }
    }
}