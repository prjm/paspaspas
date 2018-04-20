using System;
using System.Linq;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

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
        ///     compute constant value
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public override IValue ComputeValue(IValue[] inputs) {
            if (inputs.Length == 2) {

                if (Kind == DefinedOperators.ConcatOperator) {
                    return Runtime.Strings.Concat(inputs[0], inputs[1]);
                }

            }

            return null;
        }

        /// <summary>
        ///     evaluate a binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override int EvaluateBinaryOperator(Signature input) {
            if (input.Length != 2)
                return KnownTypeIds.ErrorType;

            var left = TypeRegistry.GetTypeKind(input[0]);
            var right = TypeRegistry.GetTypeKind(input[1]);

            if (Kind == DefinedOperators.ConcatOperator) {

                if ((!left.IsTextual()) || (!right.IsTextual()))
                    return KnownTypeIds.ErrorType;

                if (CommonTypeKind.WideStringType.One(left, right))
                    return KnownTypeIds.WideStringType;

                if (CommonTypeKind.AnsiCharType.One(left, right))
                    return KnownTypeIds.AnsiStringType;

                if (CommonTypeKind.LongStringType.All(left, right))
                    return KnownTypeIds.AnsiStringType;

                if (CommonTypeKind.ShortStringType.One(left, right))
                    return KnownTypeIds.AnsiStringType;

                return KnownTypeIds.UnicodeStringType;
            }

            return KnownTypeIds.ErrorType;
        }
    }
}
