using System;
using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     logical operators
    /// </summary>
    public class LogicalOperators : OperatorBase {

        private static void Register(ITypeRegistry registry, int kind, int arity = 2)
            => registry.RegisterOperator(new LogicalOperators(kind, arity));

        /// <summary>
        ///     register logical operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, DefinedOperators.NotOperation, 1);
            Register(registry, DefinedOperators.AndOperation);
            Register(registry, DefinedOperators.XorOperation);
            Register(registry, DefinedOperators.OrOperation);
            Register(registry, DefinedOperators.ShlOperation);
            Register(registry, DefinedOperators.ShrOperation);
        }

        /// <summary>
        ///     create a new logical operation
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">operator arity</param>
        public LogicalOperators(int withKind, int withArity) : base(withKind, withArity) { }

        /// <summary>
        ///     operation name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.AndOperation:
                        return "and";
                    case DefinedOperators.OrOperation:
                        return "or";
                    case DefinedOperators.XorOperation:
                        return "xor";
                    case DefinedOperators.NotOperation:
                        return "not";
                    case DefinedOperators.ShlOperation:
                        return "shl";
                    case DefinedOperators.ShrOperation:
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
        protected override ITypeReference EvaluateUnaryOperator(Signature input) {
            var operand = input[0];
            var operations = Runtime.GetLogicalOperators(operand);

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.NotOperation)
                if (operand.IsConstant)
                    return operations.Not(operand);
                else
                    return operand;

            return GetErrorTypeReference();
        }

        /// <summary>
        ///     binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override ITypeReference EvaluateBinaryOperator(Signature input) {
            var left = input[0];
            var right = input[1];

            if (Kind == DefinedOperators.ShrOperation)
                return Runtime.Integers.Shr(left, right);

            if (Kind == DefinedOperators.ShlOperation)
                return Runtime.Integers.Shl(left, right);

            var operations = Runtime.GetLogicalOperators(left, right);

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.AndOperation)
                return EvaluateAndOperator(left, right, operations);

            if (Kind == DefinedOperators.OrOperation)
                return EvaluateOrOperator(left, right, operations);

            if (Kind == DefinedOperators.XorOperation)
                return EvaluateXorOperator(left, right, operations);

            return GetErrorTypeReference();
        }

        private ITypeReference EvaluateXorOperator(ITypeReference left, ITypeReference right, ILogicalOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.Xor(left, right);
            else
                return GetSmallestBoolOrIntegralType(left, right, 8);
        }

        private ITypeReference EvaluateOrOperator(ITypeReference left, ITypeReference right, ILogicalOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.Or(left, right);
            else
                return GetSmallestBoolOrIntegralType(left, right, 8);
        }

        private ITypeReference EvaluateAndOperator(ITypeReference left, ITypeReference right, ILogicalOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.And(left, right);
            else
                return GetSmallestBoolOrIntegralType(left, right, 8);
        }
    }
}