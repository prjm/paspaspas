using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     relational operators
    /// </summary>
    public class RelationalOperator : OperatorBase {

        /// <summary>
        ///     helper function: register an relational operator
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="kind">operator kind to register</param>
        private static void Register(ITypeRegistry registry, int kind)
            => registry.RegisterOperator(new RelationalOperator(kind, 2));

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
        //
        /// <summary>
        ///     create a new relational operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">operator arity</param>
        public RelationalOperator(int withKind, int withArity)
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
        protected override ITypeSymbol EvaluateBinaryOperator(Signature input) {
            var left = input[0];
            var right = input[1];

            if (left is IEnumeratedValue leftEnum)
                left = leftEnum.Value;

            if (right is IEnumeratedValue rightEnum)
                right = rightEnum.Value;

            var operations = Runtime.GetRelationalOperators(TypeRegistry, left, right);
            if (operations == null)
                return Invalid;

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

            return Invalid;
        }

        private ITypeSymbol EvaluteGreaterThenOrEqualOperator(ITypeSymbol left, ITypeSymbol right, IRelationalOperations operations) {
            if (left.TypeDefinition.BaseType == BaseType.Set && right.TypeDefinition.BaseType == BaseType.Set && !TypeRegistry.HaveSetsCommonBaseType(left.TypeDefinition, right.TypeDefinition))
                return Invalid;

            if (left.IsConstant(out var leftConstant) && right.IsConstant(out var rightConstant))
                return operations.GreaterThenEqual(leftConstant, rightConstant);
            else
                return SystemUnit.BooleanType;
        }

        private ITypeSymbol EvaluateLessThenOrEqualOperator(ITypeSymbol left, ITypeSymbol right, IRelationalOperations operations) {
            if (left.TypeDefinition.BaseType == BaseType.Set && right.TypeDefinition.BaseType == BaseType.Set && !TypeRegistry.HaveSetsCommonBaseType(left.TypeDefinition, right.TypeDefinition))
                return Invalid;

            if (left.IsConstant(out var leftConstant) && right.IsConstant(out var rightConstant))
                return operations.LessThenOrEqual(leftConstant, rightConstant);
            else
                return SystemUnit.BooleanType;
        }

        private ITypeSymbol EvaluateGreaterThenOperator(ITypeSymbol left, ITypeSymbol right, IRelationalOperations operations) {
            if (left.IsConstant(out var leftConstant) && right.IsConstant(out var rightConstant))
                return operations.GreaterThen(leftConstant, rightConstant);
            else
                return SystemUnit.BooleanType;
        }

        private ITypeSymbol EvaluateLessThenOperator(ITypeSymbol left, ITypeSymbol right, IRelationalOperations operations) {
            if (left.IsConstant(out var leftConstant) && right.IsConstant(out var rightConstant))
                return operations.LessThen(leftConstant, rightConstant);
            else
                return SystemUnit.BooleanType;
        }

        private ITypeSymbol EvaluateNotEqualsOperator(ITypeSymbol left, ITypeSymbol right, IRelationalOperations operations) {
            if (left.TypeDefinition.BaseType == BaseType.Set && right.TypeDefinition.BaseType == BaseType.Set && !TypeRegistry.HaveSetsCommonBaseType(left.TypeDefinition, right.TypeDefinition))
                return Invalid;

            if (left.IsConstant(out var leftConstant) && right.IsConstant(out var rightConstant))
                return operations.NotEquals(leftConstant, rightConstant);
            else
                return SystemUnit.BooleanType;
        }

        private ITypeSymbol EvaluateEqualsOperator(ITypeSymbol left, ITypeSymbol right, IRelationalOperations operations) {
            if (left.TypeDefinition.BaseType == BaseType.Set && right.TypeDefinition.BaseType == BaseType.Set && !TypeRegistry.HaveSetsCommonBaseType(left.TypeDefinition, right.TypeDefinition))
                return Invalid;

            if (left.IsConstant(out var leftConstant) && right.IsConstant(out var rightConstant))
                return operations.Equal(leftConstant, rightConstant);
            else
                return SystemUnit.BooleanType;
        }
    }
}
