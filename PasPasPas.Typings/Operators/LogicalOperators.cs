using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     <c>not</c> operation
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
            var operations = Runtime.GetLogicalOperators(GetTypeKind(operand));

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.NotOperation)
                return operations.Not(operand);

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

            var operations = Runtime.GetLogicalOperators(GetTypeKind(left), GetTypeKind(right));

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.AndOperation)
                return operations.And(left, right);

            if (Kind == DefinedOperators.OrOperation)
                return operations.Or(left, right);

            if (Kind == DefinedOperators.XorOperation)
                return operations.Xor(left, right);

            return GetErrorTypeReference();
        }
    }
}