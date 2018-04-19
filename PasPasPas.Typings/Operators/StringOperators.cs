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

        /// <summary>
        ///     create a new string operator
        /// </summary>
        /// <param name="withKind"></param>
        public StringOperators(int withKind) : base(withKind) {
        }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.ConcatOperation:
                        return "+";
                }
                throw new InvalidOperationException();
            }
        }


        /// <summary>
        ///     get the output type for a given operator signature
        /// </summary>
        /// <param name="input">operator signature</param>
        /// <param name="values">current values (if constant)</param>
        /// <returns></returns>
        public override int GetOutputTypeForOperation(Signature input, object[] values) {

            if (input.Length != 2)
                return KnownTypeIds.ErrorType;

            var left = TypeRegistry.GetTypeKind(input[0]);
            var right = TypeRegistry.GetTypeKind(input[1]);

            if (Kind == DefinedOperators.ConcatOperation) {

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

        /// <summary>
        ///     register known operators
        /// </summary>
        /// <param name="typeRegistry">type registry</param>
        public static void RegisterOperators(ITypeRegistry typeRegistry)
            => typeRegistry.RegisterOperator(new StringOperators(DefinedOperators.ConcatOperation));

        /// <summary>
        ///     compute constant value
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public override IValue ComputeValue(IValue[] inputs) {
            if (inputs.Length == 2) {

                if (Kind == DefinedOperators.ConcatOperation) {
                    return Runtime.Strings.Concat(inputs[0], inputs[1]);
                }

            }

            return null;
        }
    }
}
