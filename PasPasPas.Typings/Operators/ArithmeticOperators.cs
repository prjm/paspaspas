using System;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     arithmetic operators
    /// </summary>
    public class ArithmeticOperators : OperatorBase {

        /// <summary>
        ///     create a new arithmetic operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        public ArithmeticOperators(int withKind) : base(withKind) {
        }

        /// <summary>
        ///     get the operator nae
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.PlusOperation:
                        return "+";
                    case DefinedOperators.MinusOperation:
                        return "-";
                    case DefinedOperators.TimesOperation:
                        return "*";
                    case DefinedOperators.DivOperation:
                        return "div";
                    case DefinedOperators.ModOperation:
                        return "mod";
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     get the
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override int GetOutputTypeForOperation(Signature input) {

            if (input.Length == 2 && (
                Kind == DefinedOperators.PlusOperation ||
                Kind == DefinedOperators.MinusOperation ||
                Kind == DefinedOperators.TimesOperation)) {


            }

            if (input.Length == 2 && (
                Kind == DefinedOperators.DivOperation ||
                Kind == DefinedOperators.ModOperation)) {

            }

            /*
            if (Kind == DefinedOperators.PlusOperation && input.EqualsType(Def))

            switch (Kind) {
                case DefinedOperators.PlusOperation:
                    return "+";
                case DefinedOperators.MinusOperation:
                    return "-";
                case DefinedOperators.TimesOperation:
                    return "*";
                case DefinedOperators.DivOperation:
                    return "div";
                case DefinedOperators.ModOperation:
                    return "mod";
            }
                        */
            return TypeIds.ErrorType;
        }
    }
}
