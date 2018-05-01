using System;
using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     relational operators
    /// </summary>
    public class RelationalOperators : OperatorBase {

        private static void Register(ITypeRegistry registry, int kind)
            => registry.RegisterOperator(new RelationalOperators(kind, 2));

        /// <summary>
        ///     register relational operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, DefinedOperators.EqualsOperator);
            Register(registry, DefinedOperators.NotEqualsOperator);
            Register(registry, DefinedOperators.LessThen);
            Register(registry, DefinedOperators.GreaterThen);
            Register(registry, DefinedOperators.LessThenOrEqual);
            Register(registry, DefinedOperators.GreaterThenEqual);
        }

        /// <summary>
        ///     create a new relational operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">operator arity</param>
        public RelationalOperators(int withKind, int withArity) : base(withKind, withArity) { }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.EqualsOperator:
                        return "=";
                    case DefinedOperators.NotEqualsOperator:
                        return "<>";
                    case DefinedOperators.LessThen:
                        return "<";
                    case DefinedOperators.GreaterThen:
                        return ">";
                    case DefinedOperators.LessThenOrEqual:
                        return "<=";
                    case DefinedOperators.GreaterThenEqual:
                        return ">=";
                }
                throw new InvalidOperationException();
            }
        }


        /// <summary>
        ///     evaluate a binary relational operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override ITypeReference EvaluateBinaryOperator(Signature input) {
            var left = input[0];
            var right = input[1];
            var operations = Runtime.GetRelationalOperators(left, right);

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.EqualsOperator)
                return operations.Equal(left, right);

            if (Kind == DefinedOperators.NotEqualsOperator)
                return operations.NotEquals(left, right);

            if (Kind == DefinedOperators.LessThen)
                return operations.LessThen(left, right);

            if (Kind == DefinedOperators.GreaterThen)
                return operations.GreaterThen(left, right);

            if (Kind == DefinedOperators.LessThenOrEqual)
                return operations.LessThenOrEqual(left, right);

            if (Kind == DefinedOperators.GreaterThenEqual)
                return operations.GreaterThenEqual(left, right);


            return GetErrorTypeReference();
        }
    }
}
