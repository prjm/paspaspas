using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     relational operators
    /// </summary>
    public class RelationalOperators : OperatorBase {

        /// <summary>
        ///     helper function: register an relational operator
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="kind">operator kind to register</param>
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
        public RelationalOperators(int withKind, int withArity)
            : base(withKind, withArity) { }

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
        /// <param name="input">operator input</param>
        /// <returns>operator result - constant value or types</returns>
        protected override ITypeReference EvaluateBinaryOperator(Signature input) {
            var left = input[0];
            var right = input[1];
            var operations = Runtime.GetRelationalOperators(left, right);

            if (operations == null)
                return GetErrorTypeReference();

            if (Kind == DefinedOperators.EqualsOperator)
                return EvaluateEqualsOperator(left, right, operations);

            if (Kind == DefinedOperators.NotEqualsOperator)
                return EvaluateNotEqualsOperator(left, right, operations);

            if (Kind == DefinedOperators.LessThen)
                return EvaluateLessThenOperator(left, right, operations);

            if (Kind == DefinedOperators.GreaterThen)
                return EvaluateGreaterThenOperator(left, right, operations);

            if (Kind == DefinedOperators.LessThenOrEqual)
                return EvaluateLessThenOrEqualOperator(left, right, operations);

            if (Kind == DefinedOperators.GreaterThenEqual)
                return EvaluteGreaterThenOrEqualOperator(left, right, operations);

            return GetErrorTypeReference();
        }

        private ITypeReference EvaluteGreaterThenOrEqualOperator(ITypeReference left, ITypeReference right, IRelationalOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.GreaterThenEqual(left, right);
            else
                return TypeRegistry.MakeReference(KnownTypeIds.BooleanType);
        }

        private ITypeReference EvaluateLessThenOrEqualOperator(ITypeReference left, ITypeReference right, IRelationalOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.LessThenOrEqual(left, right);
            else
                return TypeRegistry.MakeReference(KnownTypeIds.BooleanType);
        }

        private ITypeReference EvaluateGreaterThenOperator(ITypeReference left, ITypeReference right, IRelationalOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.GreaterThen(left, right);
            else
                return TypeRegistry.MakeReference(KnownTypeIds.BooleanType);
        }

        private ITypeReference EvaluateLessThenOperator(ITypeReference left, ITypeReference right, IRelationalOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.LessThen(left, right);
            else
                return TypeRegistry.MakeReference(KnownTypeIds.BooleanType);
        }

        private ITypeReference EvaluateNotEqualsOperator(ITypeReference left, ITypeReference right, IRelationalOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.NotEquals(left, right);
            else
                return TypeRegistry.MakeReference(KnownTypeIds.BooleanType);
        }

        private ITypeReference EvaluateEqualsOperator(ITypeReference left, ITypeReference right, IRelationalOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.Equal(left, right);
            else
                return TypeRegistry.MakeReference(KnownTypeIds.BooleanType);
        }
    }
}
