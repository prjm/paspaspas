using System;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     set operators
    /// </summary>
    public class SetOperator : OperatorBase {

        /// <summary>
        ///     helper function: register an operator
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="kind">operator kind</param>
        private static void Register(ITypeRegistry registry, OperatorKind kind)
            => registry.RegisterOperator(new SetOperator(kind));


        /// <summary>
        ///     register relational operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, OperatorKind.SetAddOperator);
            Register(registry, OperatorKind.SetDifferenceOperator);
            Register(registry, OperatorKind.SetIntersectOperator);
            Register(registry, OperatorKind.InSetOperator);
        }

        /// <summary>
        ///     create a new arithmetic operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        public SetOperator(OperatorKind withKind)
            : base(withKind, 2) { }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case OperatorKind.SetAddOperator:
                        return KnownNames.Plus;
                    case OperatorKind.SetDifferenceOperator:
                        return KnownNames.Minus;
                    case OperatorKind.SetIntersectOperator:
                        return KnownNames.Star;
                    case OperatorKind.InSetOperator:
                        return KnownNames.InOperator;
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     evaluate set operators
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentUnit">current unit</param>
        /// <returns></returns>
        protected override ITypeSymbol EvaluateBinaryOperator(ISignature input, IUnitType currentUnit) {
            var left = input[0];
            var right = input[1];

            if (Kind == OperatorKind.SetAddOperator)
                return EvaluateSetAddOperator(left, right, currentUnit);

            if (Kind == OperatorKind.SetDifferenceOperator)
                return EvaluateSetDiffOperator(left, right, currentUnit);

            if (Kind == OperatorKind.SetIntersectOperator)
                return EvaluateSetIntersectOperator(left, right, currentUnit);

            if (Kind == OperatorKind.InSetOperator)
                return EvaluateInSetOperator(left, right);

            return Invalid;
        }

        private ITypeSymbol EvaluateInSetOperator(ITypeSymbol left, ITypeSymbol right) {

            if (!(right.TypeDefinition.ResolveAlias() is ISetType setType))
                return Invalid;

            if (!(left.TypeDefinition.ResolveAlias() is IOrdinalType ordinalType))
                return Invalid;

            if (left is ISubrangeType subrangeType)
                if (subrangeType.ResolveAlias() is IOrdinalType subRangeBase)
                    ordinalType = subRangeBase;
                else
                    return Invalid;

            var setBaseType = setType.BaseTypeDefinition.ResolveAlias();

            if (ordinalType.BaseType == BaseType.Char && setBaseType.BaseType == BaseType.Char ||
                ordinalType.BaseType == BaseType.Boolean && setBaseType.BaseType == BaseType.Boolean ||
                ordinalType.BaseType == BaseType.Integer && setBaseType.BaseType == BaseType.Integer ||
                ordinalType.BaseType == BaseType.Enumeration && setBaseType.Equals(ordinalType)) {

                if (left.IsConstant(out var leftValue) && right.IsConstant(out var rightSet))
                    return Runtime.Structured.InSet(TypeRegistry, leftValue, rightSet);

                return TypeRegistry.SystemUnit.BooleanType.Reference;
            }

            return Invalid;
        }

        private ITypeSymbol EvaluateSetDiffOperator(ITypeSymbol left, ITypeSymbol right, IUnitType currentUnit) {
            if (left.IsConstant(out var l) && right.IsConstant(out var r))
                return Runtime.Structured.SetDifference(TypeRegistry, l, r);

            var typeCreator = TypeRegistry.CreateTypeFactory(currentUnit);
            return TypeRegistry.GetMatchingSetType(typeCreator, left.TypeDefinition, right.TypeDefinition).Reference;
        }

        private ITypeSymbol EvaluateSetAddOperator(ITypeSymbol left, ITypeSymbol right, IUnitType currentUnit) {
            if (left.IsConstant(out var leftSet) && right.IsConstant(out var rightSet))
                return Runtime.Structured.SetUnion(currentUnit, TypeRegistry, leftSet, rightSet);

            var typeCreator = TypeRegistry.CreateTypeFactory(currentUnit);
            return TypeRegistry.GetMatchingSetType(typeCreator, left.TypeDefinition, right.TypeDefinition).Reference;
        }

        private ITypeSymbol EvaluateSetIntersectOperator(ITypeSymbol left, ITypeSymbol right, IUnitType currentUnit) {
            if (left.IsConstant(out var leftSet) && right.IsConstant(out var rightSet))
                return Runtime.Structured.SetIntersection(currentUnit, TypeRegistry, leftSet, rightSet);

            var typeCreator = TypeRegistry.CreateTypeFactory(currentUnit);
            return TypeRegistry.GetMatchingSetType(typeCreator, left.TypeDefinition, right.TypeDefinition).Reference;
        }
    }
}
