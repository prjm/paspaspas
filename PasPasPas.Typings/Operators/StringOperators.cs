using System;
using System.Linq;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     string operators
    /// </summary>
    public class StringOperators : OperatorBase {

        private static void Register(ITypeRegistry registry, int kind)
            => registry.RegisterOperator(new StringOperators(kind, 2));

        /// <summary>
        ///     register known operators
        /// </summary>
        /// <param name="typeRegistry">type registry</param>
        public static void RegisterOperators(ITypeRegistry typeRegistry) {
            Register(typeRegistry, DefinedOperators.ConcatOperator);
        }

        /// <summary>
        ///     create a new string operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="arity">operator arity</param>
        public StringOperators(int withKind, int arity) : base(withKind, arity) { }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.ConcatOperator:
                        return "+";
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     evaluate a binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override ITypeReference EvaluateBinaryOperator(Signature input) {
            var left = input[0];
            var right = input[1];
            var operations = Runtime.GetStringOperators(GetTypeKind(left), GetTypeKind(right));

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.ConcatOperator)
                return operations.Concat(left, right);

            return GetErrorTypeReference();
        }
    }
}
