using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
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
        ///     create a new arithmetic operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        public ArithmeticOperators(int withKind) : base(withKind) { }

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
                return KnownTypeIds.ErrorType;

            if (input.Length == 1) {

                var operand = TypeRegistry.GetTypeKind(input[0]);

                if (Kind == DefinedOperators.UnaryPlus) {

                    if (operand == CommonTypeKind.FloatType)
                        return KnownTypeIds.Extended;

                    if (operand == CommonTypeKind.Int64Type)
                        return KnownTypeIds.Int64Type;

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
                            return KnownTypeIds.ErrorType;
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
                        return KnownTypeIds.Extended;

                    if (CommonTypeKind.Int64Type.One(left, right) && left.IsNumerical() && right.IsNumerical())
                        return KnownTypeIds.Int64Type;

                    if (CommonTypeKind.IntegerType.All(left, right))
                        return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0], input[1]);

                }

                if (Kind.In(DefinedOperators.DivOperation,
                            DefinedOperators.ModOperation)) {

                    if (CommonTypeKind.Int64Type.One(left, right) && left.IsNumerical() && right.IsNumerical())
                        return KnownTypeIds.Int64Type;

                    if (CommonTypeKind.IntegerType.All(left, right))
                        return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0], input[1]);

                }

                if (Kind == DefinedOperators.SlashOperation) {

                    if (CommonTypeKind.FloatType.One(left, right) && left.IsNumerical() && right.IsNumerical())
                        return KnownTypeIds.Extended;

                    if (CommonTypeKind.Int64Type.One(left, right) && left.IsNumerical() && right.IsNumerical())
                        return KnownTypeIds.Extended;

                    if (CommonTypeKind.IntegerType.All(left, right))
                        return KnownTypeIds.Extended;


                }

            }

            return KnownTypeIds.ErrorType;
        }

        internal static void RegisterOperators(IRuntimeValueFactory runtime, RegisteredTypes registeredTypes)
            => throw new NotImplementedException();

        /// <summary>
        ///     register known operators
        /// </summary>
        /// <param name="typeRegistry">type registry</param>
        public static void RegisterOperators(ITypeRegistry typeRegistry) {
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.UnaryMinus));
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.UnaryPlus));
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.PlusOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.MinusOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.TimesOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.DivOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.ModOperation));
            typeRegistry.RegisterOperator(new ArithmeticOperators(DefinedOperators.SlashOperation));
        }

        /// <summary>
        ///     computer operator value
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public override IValue ComputeValue(IValue[] inputs) {

            if (inputs.Length == 1) {
                return ComputeUnaryOperator(inputs[0]);
            }

            if (inputs.Length == 2)
                return ComputeBinaryOperator(inputs[0], inputs[1]);

            return null;
        }

        private IValue ComputeUnaryOperator(IValue value) {
            var number = value as INumericalValue;

            if (Kind == DefinedOperators.UnaryPlus)
                return number;

            if (Kind == DefinedOperators.UnaryMinus && value is IIntegerValue)
                return Runtime.Integers.Negate(value);

            if (Kind == DefinedOperators.UnaryMinus && value is IRealNumberValue)
                return Runtime.RealNumbers.Negate(value);

            return null;
        }

        private IValue ComputeBinaryOperator(IValue value1, IValue value2) {
            var ints = Runtime.Integers;
            var floats = Runtime.RealNumbers;

            if (value1 is IRealNumberValue || value2 is IRealNumberValue) {
                if (Kind == DefinedOperators.PlusOperation)
                    return floats.Add(value1, value2);

                if (Kind == DefinedOperators.MinusOperation)
                    return floats.Subtract(value1, value2);

                if (Kind == DefinedOperators.TimesOperation)
                    return floats.Multiply(value1, value2);

                if (Kind == DefinedOperators.SlashOperation)
                    return floats.Divide(value1, value2);

            }

            if (Kind == DefinedOperators.PlusOperation)
                return ints.Add(value1, value2);

            if (Kind == DefinedOperators.MinusOperation)
                return ints.Subtract(value1, value2);

            if (Kind == DefinedOperators.TimesOperation)
                return ints.Multiply(value1, value2);

            if (Kind == DefinedOperators.DivOperation)
                return ints.Divide(value1, value2);

            if (Kind == DefinedOperators.ModOperation)
                return ints.Modulo(value1, value2);

            if (Kind == DefinedOperators.SlashOperation)
                return floats.Divide(value1, value2);

            return null;
        }
    }
}