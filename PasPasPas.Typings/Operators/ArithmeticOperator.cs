using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     arithmetic operators
    /// </summary>
    public class ArithmeticOperator : OperatorBase {

        private static void Register(ITypeRegistry registry, int kind, int arity = 2)
            => registry.RegisterOperator(new ArithmeticOperator(kind, arity));

        /// <summary>
        ///     register known operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, DefinedOperators.UnaryMinus, 1);
            Register(registry, DefinedOperators.UnaryPlus, 1);
            Register(registry, DefinedOperators.PlusOperation);
            Register(registry, DefinedOperators.MinusOperation);
            Register(registry, DefinedOperators.TimesOperation);
            Register(registry, DefinedOperators.DivOperation);
            Register(registry, DefinedOperators.ModOperation);
            Register(registry, DefinedOperators.SlashOperation);
        }

        /// <summary>
        ///     create a new arithmetic operator
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">arity</param>
        public ArithmeticOperator(int withKind, int withArity = 2) : base(withKind, withArity) { }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.UnaryPlus:
                        return "+";
                    case DefinedOperators.UnaryMinus:
                        return "-";
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
                    case DefinedOperators.SlashOperation:
                        return "/";
                }
                throw new InvalidOperationException();
            }
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

        /// <summary>
        ///     evaluate a unary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override int EvaluateUnaryOperator(Signature input) {

            var operand = TypeRegistry.GetTypeKind(input[0].TypeId);

            if (Kind == DefinedOperators.UnaryPlus) {

                if (operand == CommonTypeKind.FloatType)
                    return KnownTypeIds.Extended;

                if (operand == CommonTypeKind.Int64Type)
                    return KnownTypeIds.Int64Type;

                if (operand == CommonTypeKind.IntegerType)
                    return input[0].TypeId;

            }
            if (Kind == DefinedOperators.UnaryMinus) {

                if (operand == CommonTypeKind.FloatType)
                    return input[0].TypeId;

                if (operand == CommonTypeKind.IntegerType || operand == CommonTypeKind.Int64Type) {
                    if (ResolveAlias(input[0].TypeId) is IIntegralType currentType)
                        return currentType.TypeId;
                    else
                        return KnownTypeIds.ErrorType;
                }
            }

            return KnownTypeIds.ErrorType;
        }

        /// <summary>
        ///     evaluate a binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override int EvaluateBinaryOperator(Signature input) {

            var left = TypeRegistry.GetTypeKind(input[0].TypeId);
            var right = TypeRegistry.GetTypeKind(input[1].TypeId);

            if (Kind.In(DefinedOperators.PlusOperation,
                        DefinedOperators.MinusOperation,
                        DefinedOperators.TimesOperation)) {

                if (CommonTypeKind.FloatType.One(left, right) && left.IsNumerical() && right.IsNumerical())
                    return KnownTypeIds.Extended;

                if (CommonTypeKind.Int64Type.One(left, right) && left.IsNumerical() && right.IsNumerical())
                    return KnownTypeIds.Int64Type;

                if (CommonTypeKind.IntegerType.All(left, right))
                    return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0].TypeId, input[1].TypeId);

            }

            if (Kind.In(DefinedOperators.DivOperation,
                        DefinedOperators.ModOperation)) {

                if (CommonTypeKind.Int64Type.One(left, right) && left.IsNumerical() && right.IsNumerical())
                    return KnownTypeIds.Int64Type;

                if (CommonTypeKind.IntegerType.All(left, right))
                    return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0].TypeId, input[1].TypeId);

            }

            if (Kind == DefinedOperators.SlashOperation) {

                if (CommonTypeKind.FloatType.One(left, right) && left.IsNumerical() && right.IsNumerical())
                    return KnownTypeIds.Extended;

                if (CommonTypeKind.Int64Type.One(left, right) && left.IsNumerical() && right.IsNumerical())
                    return KnownTypeIds.Extended;

                if (CommonTypeKind.IntegerType.All(left, right))
                    return KnownTypeIds.Extended;


            }

            return KnownTypeIds.ErrorType;
        }
    }
}