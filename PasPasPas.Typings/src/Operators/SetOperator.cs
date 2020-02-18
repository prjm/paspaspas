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
        private static void Register(ITypeRegistry registry, int kind)
            => registry.RegisterOperator(new SetOperator(kind));


        /// <summary>
        ///     register relational operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, DefinedOperators.SetAddOperator);
            Register(registry, DefinedOperators.SetDifferenceOperator);
            Register(registry, DefinedOperators.SetIntersectOperator);
            Register(registry, DefinedOperators.InSetOperator);
        }

        /// <summary>
        ///     create a new arithmetic operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        public SetOperator(int withKind)
            : base(withKind, 2) { }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.SetAddOperator:
                        return "+";
                    case DefinedOperators.SetDifferenceOperator:
                        return "-";
                    case DefinedOperators.SetIntersectOperator:
                        return "*";
                    case DefinedOperators.InSetOperator:
                        return "in";
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     evaluate set operators
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override ITypeSymbol EvaluateBinaryOperator(Signature input) {
            var left = input[0];
            var right = input[1];

            if (Kind == DefinedOperators.SetAddOperator)
                return EvaluateSetAddOperator(left, right);

            if (Kind == DefinedOperators.SetDifferenceOperator)
                return EvaluateSetDiffOperator(left, right);

            if (Kind == DefinedOperators.SetIntersectOperator)
                return EvaluateSetIntersectOperator(left, right);

            if (Kind == DefinedOperators.InSetOperator)
                return EvaluateInSetOperator(left, right);

            return Invalid;
        }

        private ITypeSymbol EvaluateInSetOperator(ITypeSymbol left, ITypeSymbol right) {

            if (!(right.TypeDefinition.ResolveAlias() is ISetType setType))
                return Invalid;

            if (!(left.TypeDefinition.ResolveAlias() is IOrdinalType ordinalType))
                return Invalid;

            if (left is ISubrangeType subrangeType)
                if (subrangeType.TypeDefinition.ResolveAlias() is IOrdinalType subRangeBase)
                    ordinalType = subrangeType;
                else
                    return Invalid;

            var setBaseType = setType.BaseTypeDefinition.ResolveAlias();

            if (ordinalType.TypeKind.IsChar() && setBaseType.TypeKind.IsChar() ||
                ordinalType.TypeKind == CommonTypeKind.BooleanType && setBaseType.TypeKind == CommonTypeKind.BooleanType ||
                ordinalType.TypeKind.IsNumerical() && setBaseType.TypeKind.IsNumerical() ||
                ordinalType.TypeKind == CommonTypeKind.EnumerationType && setBaseType.TypeId == ordinalType.TypeId) {

                if (left.IsConstant() && right.IsConstant())
                    return Runtime.Structured.InSet(TypeRegistry, left, right);

                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.BooleanType);
            }

            return GetErrorTypeReference();
        }

        private ITypeSymbol EvaluateSetDiffOperator(ITypeSymbol left, ITypeSymbol right) {
            if (left.IsConstant() && right.IsConstant())
                return Runtime.Structured.SetDifference(TypeRegistry, left, right);
            else
                return TypeRegistry.GetMatchingSetType(left, right);
        }

        private ITypeSymbol ference EvaluateSetAddOperator(ITypeSymbol left, ITypeSymbol right) {
            if (left.IsConstant() && right.IsConstant())
                return Runtime.Structured.SetUnion(TypeRegistry, left, right);
            else
                return TypeRegistry.GetMatchingSetType(left, right);
        }

        private ITypeSymbol EvaluateSetIntersectOperator(ITypeSymbol left, ITypeSymbol right) {
            if (left.IsConstant() && right.IsConstant())
                return Runtime.Structured.SetIntersection(TypeRegistry, left, right);
            else
                return TypeRegistry.GetMatchingSetType(left, right);
        }
    }
}
