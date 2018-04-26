using System;
using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     arithmetic operators
    /// </summary>
    public class ArithmeticOperator : OperatorBase {

        private static void Register(ITypeRegistry registry, int kind, int arity = 2)
            => registry.RegisterOperator(new ArithmeticOperator(kind, arity));

        /// <summary>
        ///     register known operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, DefinedOperators.UnaryMinus, 1);
            Register(registry, DefinedOperators.UnaryPlus, 1);
            Register(registry, DefinedOperators.PlusOperation);
            Register(registry, DefinedOperators.MinusOperation);
            Register(registry, DefinedOperators.TimesOperation);
            Register(registry, DefinedOperators.DivOperation);
            Register(registry, DefinedOperators.ModOperation);
            Register(registry, DefinedOperators.SlashOperation);
        }

        /// <summary>
        ///     create a new arithmetic operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">arity</param>
        public ArithmeticOperator(int withKind, int withArity = 2) : base(withKind, withArity) { }

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
                    case DefinedOperators.PlusOperation:
                        return "+";
                    case DefinedOperators.MinusOperation:
                        return "-";
                    case DefinedOperators.TimesOperation:
                        return "*";
                    case DefinedOperators.DivOperation:
                        return "div";
                    case DefinedOperators.ModOperation:
                        return "mod";
                    case DefinedOperators.SlashOperation:
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
            var operations = Runtime.GetArithmeticOperators(GetTypeKind(operand));

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.UnaryPlus)
                return operations.Identity(operand);

            if (Kind == DefinedOperators.UnaryMinus)
                return operations.Negate(operand);

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

            if (Kind == DefinedOperators.DivOperation)
                return Runtime.Integers.Divide(left, right);

            if (Kind == DefinedOperators.ModOperation)
                return Runtime.Integers.Modulo(left, right);

            if (Kind == DefinedOperators.SlashOperation)
                return Runtime.RealNumbers.Divide(left, right);

            var operations = Runtime.GetArithmeticOperators(GetTypeKind(left), GetTypeKind(right));

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.PlusOperation)
                return operations.Add(left, right);

            if (Kind == DefinedOperators.MinusOperation)
                return operations.Subtract(left, right);

            if (Kind == DefinedOperators.TimesOperation)
                return operations.Multiply(left, right);

            return GetErrorTypeReference();
        }
    }
}