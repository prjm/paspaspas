using System;
using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;

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
                return EvaluateConcatOperator(left, right, operations);

            return GetErrorTypeReference();
        }

        private ITypeReference EvaluateConcatOperator(ITypeReference left, ITypeReference right, IStringOperations operations) {
            if (left.IsConstant && right.IsConstant)
                return operations.Concat(left, right);

            var leftType = TypeRegistry.GetTypeByIdOrUndefinedType(left.TypeId);
            var rightType = TypeRegistry.GetTypeByIdOrUndefinedType(right.TypeId);

            if (leftType.TypeKind == CommonTypeKind.UnicodeStringType || right.TypeKind == CommonTypeKind.UnicodeStringType)
                return TypeRegistry.MakeReference(KnownTypeIds.UnicodeStringType);

            if (leftType.TypeKind == CommonTypeKind.WideCharType || rightType.TypeKind == CommonTypeKind.WideCharType)
                return TypeRegistry.MakeReference(KnownTypeIds.UnicodeStringType);

            return TypeRegistry.MakeReference(KnownTypeIds.AnsiStringType);
        }
    }
}
