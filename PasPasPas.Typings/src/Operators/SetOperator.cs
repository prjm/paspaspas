using System;
using PasPasPas.Globals.Runtime;
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
        protected override ITypeReference EvaluateBinaryOperator(Signature input) {
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

            return GetErrorTypeReference();
        }

        private ITypeReference EvaluateInSetOperator(ITypeReference left, ITypeReference right) {

            if (!(TypeRegistry.ResolveAlias(right.TypeId) is ISetType setType))
                return GetErrorTypeReference();

            if (!(TypeRegistry.ResolveAlias(left.TypeId) is IOrdinalType ordinalType))
                return GetErrorTypeReference();

            if (left is ISubrangeType subrangeType)
                if (TypeRegistry.ResolveAlias(subrangeType.BaseTypeId) is IOrdinalType subRangeBase)
                    ordinalType = subrangeType;
                else
                    return GetErrorTypeReference();

            var setBaseType = TypeRegistry.ResolveAlias(setType.BaseTypeId);

            if ((ordinalType.TypeKind.IsChar() && setBaseType.TypeKind.IsChar()) ||
                (ordinalType.TypeKind == CommonTypeKind.BooleanType && setBaseType.TypeKind == CommonTypeKind.BooleanType) ||
                (ordinalType.TypeKind.IsNumerical() && setBaseType.TypeKind.IsNumerical()) ||
                (ordinalType.TypeKind == CommonTypeKind.EnumerationType && setBaseType.TypeId == ordinalType.TypeId)) {

                if (left.IsConstant() && right.IsConstant())
                    return Runtime.Structured.InSet(TypeRegistry, left, right);

                return TypeRegistry.MakeReference(KnownTypeIds.BooleanType);
            }

            return GetErrorTypeReference();
        }

        private ITypeReference EvaluateSetDiffOperator(ITypeReference left, ITypeReference right) {
            if (left.IsConstant() && right.IsConstant())
                return Runtime.Structured.SetDifference(TypeRegistry, left, right);
            else
                return TypeRegistry.GetMatchingSetType(left, right);
        }

        private ITypeReference EvaluateSetAddOperator(ITypeReference left, ITypeReference right) {
            if (left.IsConstant() && right.IsConstant())
                return Runtime.Structured.SetUnion(TypeRegistry, left, right);
            else
                return TypeRegistry.GetMatchingSetType(left, right);
        }

        private ITypeReference EvaluateSetIntersectOperator(ITypeReference left, ITypeReference right) {
            if (left.IsConstant() && right.IsConstant())
                return Runtime.Structured.SetIntersection(TypeRegistry, left, right);
            else
                return TypeRegistry.GetMatchingSetType(left, right);
        }
    }
}
