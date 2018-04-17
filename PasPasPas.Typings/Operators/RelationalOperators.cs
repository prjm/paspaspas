using System;
using System.Linq;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     relational operators
    /// </summary>
    public class RelationalOperators : OperatorBase {

        /// <summary>
        ///     create a new relational operator
        /// </summary>
        /// <param name="withKind"></param>
        public RelationalOperators(int withKind) : base(withKind) {
        }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.EqualsOperator:
                        return "=";
                    case DefinedOperators.NotEqualsOperator:
                        return "<>";
                    case DefinedOperators.LessThen:
                        return "<";
                    case DefinedOperators.GreaterThen:
                        return ">";
                    case DefinedOperators.LessThenOrEqual:
                        return "<=";
                    case DefinedOperators.GreaterThenEqual:
                        return ">=";
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     get output type for operations
        /// </summary>
        /// <param name="input"></param>
        /// <param name="values">current values (if constant)</param>
        /// <returns></returns>
        public override int GetOutputTypeForOperation(Signature input, object[] values) {
            if (input.Length != 2)
                return KnownTypeIds.ErrorType;

            var left = TypeRegistry.GetTypeKind(input[0]);
            var right = TypeRegistry.GetTypeKind(input[1]);

            if (CommonTypeKind.BooleanType.All(left, right))
                return KnownTypeIds.BooleanType;

            if (left.IsNumerical() && right.IsNumerical())
                return KnownTypeIds.BooleanType;

            if (left.IsTextual() && right.IsTextual())
                return KnownTypeIds.BooleanType;


            return KnownTypeIds.ErrorType;
        }


        /// <summary>
        ///     register relational operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            registry.RegisterOperator(new RelationalOperators(DefinedOperators.EqualsOperator));
            registry.RegisterOperator(new RelationalOperators(DefinedOperators.NotEqualsOperator));
            registry.RegisterOperator(new RelationalOperators(DefinedOperators.LessThen));
            registry.RegisterOperator(new RelationalOperators(DefinedOperators.GreaterThen));
            registry.RegisterOperator(new RelationalOperators(DefinedOperators.LessThenOrEqual));
            registry.RegisterOperator(new RelationalOperators(DefinedOperators.GreaterThenEqual));
        }

        /// <summary>
        ///     compute value
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public override IValue ComputeValue(IValue[] inputs) {
            if (inputs.Length == 2)
                return ComputeBinaryOperator(inputs[0], inputs[1]);

            return null;
        }

        private IValue ComputeBinaryOperator(IValue left, IValue right) {

            if (left is IBooleanValue && right is IBooleanValue) {

                if (Kind == DefinedOperators.EqualsOperator)
                    return Runtime.BooleanCalculator.Equal(left, right);

                if (Kind == DefinedOperators.NotEqualsOperator)
                    return Runtime.BooleanCalculator.NotEquals(left, right);

                if (Kind == DefinedOperators.LessThen)
                    return Runtime.BooleanCalculator.LessThen(left, right);

                if (Kind == DefinedOperators.GreaterThen)
                    return Runtime.BooleanCalculator.GreaterThen(left, right);

                if (Kind == DefinedOperators.LessThenOrEqual)
                    return Runtime.BooleanCalculator.LessThenOrEqual(left, right);

                if (Kind == DefinedOperators.GreaterThenEqual)
                    return Runtime.BooleanCalculator.GreaterThenEqual(left, right);

            }

            if ((left is IRealValue && right is INumericalValue) || (left is INumericalValue && right is IRealValue)) {

                if (Kind == DefinedOperators.EqualsOperator)
                    return Runtime.FloatCalculator.Equal(left, right);

                if (Kind == DefinedOperators.NotEqualsOperator)
                    return Runtime.FloatCalculator.NotEquals(left, right);

                if (Kind == DefinedOperators.LessThen)
                    return Runtime.FloatCalculator.LessThen(left, right);

                if (Kind == DefinedOperators.GreaterThen)
                    return Runtime.FloatCalculator.GreaterThen(left, right);

                if (Kind == DefinedOperators.LessThenOrEqual)
                    return Runtime.FloatCalculator.LessThenOrEqual(left, right);

                if (Kind == DefinedOperators.GreaterThenEqual)
                    return Runtime.FloatCalculator.GreaterThenEqual(left, right);

            }


            if (left is IIntegerValue && right is IIntegerValue) {

                if (Kind == DefinedOperators.EqualsOperator)
                    return Runtime.ScaledIntegerCalculator.Equal(left, right);

                if (Kind == DefinedOperators.NotEqualsOperator)
                    return Runtime.ScaledIntegerCalculator.NotEquals(left, right);

                if (Kind == DefinedOperators.LessThen)
                    return Runtime.ScaledIntegerCalculator.LessThen(left, right);

                if (Kind == DefinedOperators.GreaterThen)
                    return Runtime.ScaledIntegerCalculator.GreaterThen(left, right);

                if (Kind == DefinedOperators.LessThenOrEqual)
                    return Runtime.ScaledIntegerCalculator.LessThenOrEqual(left, right);

                if (Kind == DefinedOperators.GreaterThenEqual)
                    return Runtime.ScaledIntegerCalculator.GreaterThenEqual(left, right);

            }

            if (left is IStringValue && right is IStringValue) {

                if (Kind == DefinedOperators.EqualsOperator)
                    return Runtime.StringCalculator.Equal(left, right);

                if (Kind == DefinedOperators.NotEqualsOperator)
                    return Runtime.StringCalculator.NotEquals(left, right);

                if (Kind == DefinedOperators.LessThen)
                    return Runtime.StringCalculator.LessThen(left, right);

                if (Kind == DefinedOperators.GreaterThen)
                    return Runtime.StringCalculator.GreaterThen(left, right);

                if (Kind == DefinedOperators.LessThenOrEqual)
                    return Runtime.StringCalculator.LessThenOrEqual(left, right);

                if (Kind == DefinedOperators.GreaterThenEqual)
                    return Runtime.StringCalculator.GreaterThenEqual(left, right);

            }

            return null;
        }
    }
}
