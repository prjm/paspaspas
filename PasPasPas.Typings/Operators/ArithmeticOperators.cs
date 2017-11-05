using System;
using PasPasPas.Infrastructure.Utils;
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

            var left = TypeRegistry.GetTypeKind(input[0]);
            var right = TypeRegistry.GetTypeKind(input[1]);

            if (Kind.In(DefinedOperators.PlusOperation,
                        DefinedOperators.MinusOperation,
                        DefinedOperators.TimesOperation)) {

                if (CommonTypeKind.FloatType.One(left, right))
                    return TypeIds.Extended;

                if (CommonTypeKind.Int64Type.One(left, right))
                    return TypeIds.Int64Type;

                if (CommonTypeKind.IntegerType.All(left, right))
                    return TypeIds.IntegerType;

            }

            if (Kind.In(DefinedOperators.DivOperation,
                        DefinedOperators.ModOperation)) {

                if (CommonTypeKind.Int64Type.One(left, right))
                    return TypeIds.Int64Type;

                if (CommonTypeKind.IntegerType.All(left, right))
                    return TypeIds.IntegerType;

            }

            if (Kind == DefinedOperators.SlashOperation) {

                if (CommonTypeKind.FloatType.One(left, right))
                    return TypeIds.Extended;

                if (CommonTypeKind.Int64Type.One(left, right))
                    return TypeIds.Extended;

                if (CommonTypeKind.IntegerType.All(left, right))
                    return TypeIds.Extended;


            }

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
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.SlashOperation));
        }
    }
}
