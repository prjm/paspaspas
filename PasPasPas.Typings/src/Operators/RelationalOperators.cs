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
        private static void Register(ITypeRegistry registry, OperatorKind kind)
            => registry.RegisterOperator(new RelationalOperator(kind, 2));

        /// <summary>
        ///     register relational operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, OperatorKind.EqualsOperator);
            Register(registry, OperatorKind.NotEqualsOperator);
            Register(registry, OperatorKind.LessThan);
            Register(registry, OperatorKind.GreaterThan);
            Register(registry, OperatorKind.LessThanOrEqual);
            Register(registry, OperatorKind.GreaterThanOrEqual);
        }
        //
        /// <summary>
        ///     create a new relational operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">operator arity</param>
        public RelationalOperator(OperatorKind withKind, int withArity)
            : base(withKind, withArity) { }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case OperatorKind.EqualsOperator:
                        return KnownNames.EqualsOperator;
                    case OperatorKind.NotEqualsOperator:
                        return KnownNames.NotEqualsOperator;
                    case OperatorKind.LessThan:
                        return KnownNames.LessThan;
                    case OperatorKind.GreaterThan:
                        return KnownNames.GreaterThan;
                    case OperatorKind.LessThanOrEqual:
                        return KnownNames.LessThanOrEqual;
                    case OperatorKind.GreaterThanOrEqual:
                        return KnownNames.GreaterThanOrEqual;
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     evaluate a binary relational operator
        /// </summary>
        /// <param name="input">operator input</param>
        /// <returns>operator result - constant value or types</returns>
        protected override ITypeSymbol EvaluateBinaryOperator(ISignature input, IUnitType currentUnit) {
            var left = input[0];
            var right = input[1];

            if (left is IEnumeratedValue leftEnum)
                left = leftEnum.Value;

            if (right is IEnumeratedValue rightEnum)
                right = rightEnum.Value;

            var operations = Runtime.GetRelationalOperators(TypeRegistry, left, right);
            if (operations == null)
                return Invalid;

            if (Kind == OperatorKind.EqualsOperator)
                return EvaluateEqualsOperator(left, right, operations);

            if (Kind == OperatorKind.NotEqualsOperator)
                return EvaluateNotEqualsOperator(left, right, operations);

            if (Kind == OperatorKind.LessThan)
                return EvaluateLessThenOperator(left, right, operations);

            if (Kind == OperatorKind.GreaterThan)
                return EvaluateGreaterThenOperator(left, right, operations);

            if (Kind == OperatorKind.LessThanOrEqual)
                return EvaluateLessThenOrEqualOperator(left, right, operations);

            if (Kind == OperatorKind.GreaterThanOrEqual)
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
