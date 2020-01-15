﻿using System;
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
        protected override IOldTypeReference EvaluateBinaryOperator(Signature input) {
            var left = input[0];
            var right = input[1];

            if (left is IEnumeratedValue leftEnum)
                left = leftEnum.Value;

            if (right is IEnumeratedValue rightEnum)
                right = rightEnum.Value;

            var operations = Runtime.GetRelationalOperators(TypeRegistry, left, right);
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

        private IOldTypeReference EvaluteGreaterThenOrEqualOperator(IOldTypeReference left, IOldTypeReference right, IRelationalOperations operations) {
            if (left.IsSet() && right.IsSet() && !TypeRegistry.HaveSetsCommonBaseType(left, right))
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.ErrorType);

            if (left.IsConstant() && right.IsConstant())
                return operations.GreaterThenEqual(left, right);
            else
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.BooleanType);
        }

        private IOldTypeReference EvaluateLessThenOrEqualOperator(IOldTypeReference left, IOldTypeReference right, IRelationalOperations operations) {
            if (left.IsSet() && right.IsSet() && !TypeRegistry.HaveSetsCommonBaseType(left, right))
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.ErrorType);

            if (left.IsConstant() && right.IsConstant())
                return operations.LessThenOrEqual(left, right);
            else
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.BooleanType);
        }

        private IOldTypeReference EvaluateGreaterThenOperator(IOldTypeReference left, IOldTypeReference right, IRelationalOperations operations) {
            if (left.IsConstant() && right.IsConstant())
                return operations.GreaterThen(left, right);
            else
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.BooleanType);
        }

        private IOldTypeReference EvaluateLessThenOperator(IOldTypeReference left, IOldTypeReference right, IRelationalOperations operations) {
            if (left.IsConstant() && right.IsConstant())
                return operations.LessThen(left, right);
            else
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.BooleanType);
        }

        private IOldTypeReference EvaluateNotEqualsOperator(IOldTypeReference left, IOldTypeReference right, IRelationalOperations operations) {
            if (left.IsSet() && right.IsSet() && !TypeRegistry.HaveSetsCommonBaseType(left, right))
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.ErrorType);

            if (left.IsConstant() && right.IsConstant())
                return operations.NotEquals(left, right);
            else
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.BooleanType);
        }

        private IOldTypeReference EvaluateEqualsOperator(IOldTypeReference left, IOldTypeReference right, IRelationalOperations operations) {
            if (left.IsSet() && right.IsSet() && !TypeRegistry.HaveSetsCommonBaseType(left, right))
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.ErrorType);

            if (left.IsConstant() && right.IsConstant())
                return operations.Equal(left, right);
            else
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.BooleanType);
        }
    }
}
