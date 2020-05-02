using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     string operators
    /// </summary>
    public class StringOperator : OperatorBase {

        private static void Register(ITypeRegistry registry, OperatorKind kind)
            => registry.RegisterOperator(new StringOperator(kind, 2));

        /// <summary>
        ///     register known operators
        /// </summary>
        /// <param name="typeRegistry">type registry</param>
        public static void RegisterOperators(ITypeRegistry typeRegistry)
            => Register(typeRegistry, OperatorKind.ConcatOperator);

        /// <summary>
        ///     create a new string operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="arity">operator arity</param>
        public StringOperator(OperatorKind withKind, int arity) : base(withKind, arity) { }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case OperatorKind.ConcatOperator:
                        return KnownNames.Plus;
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     evaluate a binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentUnit">current unit</param>
        /// <returns></returns>
        protected override ITypeSymbol EvaluateBinaryOperator(ISignature input, IUnitType currentUnit) {
            var left = input[0];
            var right = input[1];
            var operations = Runtime.GetStringOperators(left, right);

            if (operations == null)
                return Invalid;

            if (Kind == OperatorKind.ConcatOperator)
                return EvaluateConcatOperator(left, right, operations);

            return Invalid;
        }

        private ITypeSymbol EvaluateConcatOperator(ITypeSymbol left, ITypeSymbol right, IStringOperations operations) {
            if (left.IsConstant(out var leftValue) && right.IsConstant(out var rightValue))
                return operations.Concat(leftValue, rightValue);

            var leftType = left.TypeDefinition;
            var rightType = right.TypeDefinition;

            if (leftType is IStringType leftString && leftString.Kind == StringTypeKind.UnicodeString)
                return leftType.Reference;

            if (rightType is IStringType rightString && rightString.Kind == StringTypeKind.UnicodeString)
                return rightType.Reference;

            if (leftType is ICharType leftChar && leftChar.Kind == CharTypeKind.WideChar)
                return SystemUnit.UnicodeStringType.Reference;

            if (rightType is ICharType rightChar && rightChar.Kind == CharTypeKind.WideChar)
                return SystemUnit.UnicodeStringType.Reference;

            return SystemUnit.AnsiStringType.Reference;
        }
    }
}
