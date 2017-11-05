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

            if (input.Length != 2)
                return TypeIds.ErrorType;

            var left = TypeRegistry.GetTypeByIdOrUndefinedType(input[0]).TypeKind;
            var right = TypeRegistry.GetTypeByIdOrUndefinedType(input[1]).TypeKind;

            if (Kind == DefinedOperators.PlusOperation ||
                Kind == DefinedOperators.MinusOperation ||
                Kind == DefinedOperators.TimesOperation) {

                if (left == CommonTypeKind.IntegerType && right == CommonTypeKind.IntegerType)
                    return TypeIds.IntegerType;

            }

            if (input.Length == 2 && (
                Kind == DefinedOperators.DivOperation ||
                Kind == DefinedOperators.ModOperation)) {

                if (left == CommonTypeKind.IntegerType && right == CommonTypeKind.IntegerType)
                    return TypeIds.IntegerType;

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

        /// <summary>
        ///     register known 
        /// </summary>
        /// <param name="typeRegistry">type registry</param>
        public static void RegisterOperators(ITypeRegistry typeRegistry) {
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.PlusOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.MinusOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.TimesOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.DivOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.ModOperation));
        }
    }
}
