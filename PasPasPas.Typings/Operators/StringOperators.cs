using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <returns></returns>
        public override int GetOutputTypeForOperation(Signature input) {

            if (input.Length != 2)
                return TypeIds.ErrorType;

            var left = TypeRegistry.GetTypeKind(input[0]);
            var right = TypeRegistry.GetTypeKind(input[1]);

            if (Kind == DefinedOperators.ConcatOperation) {

                if ((!left.Textual()) || (!right.Textual()))
                    return TypeIds.ErrorType;

                if (CommonTypeKind.WideStringType.One(left, right))
                    return TypeIds.WideStringType;

                if (CommonTypeKind.AnsiCharType.One(left, right))
                    return TypeIds.AnsiStringType;

                if (CommonTypeKind.LongStringType.All(left, right))
                    return TypeIds.AnsiStringType;

                if (CommonTypeKind.ShortStringType.One(left, right))
                    return TypeIds.AnsiStringType;

                return TypeIds.UnicodeStringType;
            }

            return TypeIds.ErrorType;
        }

        /// <summary>
        ///     register known operators
        /// </summary>
        /// <param name="typeRegistry">type registry</param>
        public static void RegisterOperators(ITypeRegistry typeRegistry)
            => typeRegistry.RegisterOperator(new StringOperators(DefinedOperators.ConcatOperation));
    }
}
