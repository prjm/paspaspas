using System;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     arithmetic operators
    /// </summary>
    public class ArithmeticOperators : OperatorBase {

        /// <summary>
        ///     litera unwrapper
        /// </summary>
        private readonly ILiteralUnwrapper literalUnwrapper;

        /// <summary>
        ///     create a new arithmetic operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="unwrapper">unwrapper for litersl</param>
        public ArithmeticOperators(ILiteralUnwrapper unwrapper, int withKind) : base(withKind)
            => literalUnwrapper = unwrapper;

        /// <summary>
        ///     get the operator name
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
        ///     get the output type for a given operator signature
        /// </summary>
        /// <param name="input">operator signature</param>
        /// <param name="values">current value</param>
        /// <returns></returns>
        public override int GetOutputTypeForOperation(Signature input, object[] values) {

            if (!input.Length.In(1, 2))
                return TypeIds.ErrorType;

            if (input.Length == 1) {

                var operand = TypeRegistry.GetTypeKind(input[0]);

                if (Kind == DefinedOperators.UnaryPlus) {

                    if (operand == CommonTypeKind.FloatType)
                        return TypeIds.Extended;

                    if (operand == CommonTypeKind.Int64Type)
                        return TypeIds.Int64Type;

                    if (operand == CommonTypeKind.IntegerType)
                        return input[0];

                }
                if (Kind == DefinedOperators.UnaryMinus) {

                    if (operand == CommonTypeKind.FloatType)
                        return input[0];

                    if (operand == CommonTypeKind.IntegerType || operand == CommonTypeKind.Int64Type) {
                        if (ResolveAlias(input[0]) is IIntegralType currentType)
                            return currentType.TypeId;
                        else
                            return TypeIds.ErrorType;
                    }
                }
            }
            else if (input.Length == 2) {

                var left = TypeRegistry.GetTypeKind(input[0]);
                var right = TypeRegistry.GetTypeKind(input[1]);

                if (Kind.In(DefinedOperators.PlusOperation,
                            DefinedOperators.MinusOperation,
                            DefinedOperators.TimesOperation)) {

                    if (CommonTypeKind.FloatType.One(left, right) && left.IsNumerical() && right.IsNumerical())
                        return TypeIds.Extended;

                    if (CommonTypeKind.Int64Type.One(left, right) && left.IsNumerical() && right.IsNumerical())
                        return TypeIds.Int64Type;

                    if (CommonTypeKind.IntegerType.All(left, right))
                        return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0], input[1]);

                }

                if (Kind.In(DefinedOperators.DivOperation,
                            DefinedOperators.ModOperation)) {

                    if (CommonTypeKind.Int64Type.One(left, right) && left.IsNumerical() && right.IsNumerical())
                        return TypeIds.Int64Type;

                    if (CommonTypeKind.IntegerType.All(left, right))
                        return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0], input[1]);

                }

                if (Kind == DefinedOperators.SlashOperation) {

                    if (CommonTypeKind.FloatType.One(left, right) && left.IsNumerical() && right.IsNumerical())
                        return TypeIds.Extended;

                    if (CommonTypeKind.Int64Type.One(left, right) && left.IsNumerical() && right.IsNumerical())
                        return TypeIds.Extended;

                    if (CommonTypeKind.IntegerType.All(left, right))
                        return TypeIds.Extended;


                }

            }

            return TypeIds.ErrorType;
        }

        /// <summary>
        ///     register known operators
        /// </summary>
        /// <param name="unwrapper">literal unwrapper</param>
        /// <param name="typeRegistry">type registry</param>
        public static void RegisterOperators(ILiteralUnwrapper unwrapper, ITypeRegistry typeRegistry) {
            typeRegistry.RegisterOperator(new ArithmeticOperators(unwrapper, DefinedOperators.UnaryMinus));
            typeRegistry.RegisterOperator(new ArithmeticOperators(unwrapper, DefinedOperators.UnaryPlus));
            typeRegistry.RegisterOperator(new ArithmeticOperators(unwrapper, DefinedOperators.PlusOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(unwrapper, DefinedOperators.MinusOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(unwrapper, DefinedOperators.TimesOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(unwrapper, DefinedOperators.DivOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(unwrapper, DefinedOperators.ModOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(unwrapper, DefinedOperators.SlashOperation));
        }
    }
}
