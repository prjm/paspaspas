﻿using System;
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
                    return Runtime.Booleans.Equal(left, right);

                if (Kind == DefinedOperators.NotEqualsOperator)
                    return Runtime.Booleans.NotEquals(left, right);

                if (Kind == DefinedOperators.LessThen)
                    return Runtime.Booleans.LessThen(left, right);

                if (Kind == DefinedOperators.GreaterThen)
                    return Runtime.Booleans.GreaterThen(left, right);

                if (Kind == DefinedOperators.LessThenOrEqual)
                    return Runtime.Booleans.LessThenOrEqual(left, right);

                if (Kind == DefinedOperators.GreaterThenEqual)
                    return Runtime.Booleans.GreaterThenEqual(left, right);

            }

            if ((left is IRealNumberValue && right is INumericalValue) || (left is INumericalValue && right is IRealNumberValue)) {

                if (Kind == DefinedOperators.EqualsOperator)
                    return Runtime.RealNumbers.Equal(left, right);

                if (Kind == DefinedOperators.NotEqualsOperator)
                    return Runtime.RealNumbers.NotEquals(left, right);

                if (Kind == DefinedOperators.LessThen)
                    return Runtime.RealNumbers.LessThen(left, right);

                if (Kind == DefinedOperators.GreaterThen)
                    return Runtime.RealNumbers.GreaterThen(left, right);

                if (Kind == DefinedOperators.LessThenOrEqual)
                    return Runtime.RealNumbers.LessThenOrEqual(left, right);

                if (Kind == DefinedOperators.GreaterThenEqual)
                    return Runtime.RealNumbers.GreaterThenEqual(left, right);

            }


            if (left is IIntegerValue && right is IIntegerValue) {

                if (Kind == DefinedOperators.EqualsOperator)
                    return Runtime.Integers.Equal(left, right);

                if (Kind == DefinedOperators.NotEqualsOperator)
                    return Runtime.Integers.NotEquals(left, right);

                if (Kind == DefinedOperators.LessThen)
                    return Runtime.Integers.LessThen(left, right);

                if (Kind == DefinedOperators.GreaterThen)
                    return Runtime.Integers.GreaterThen(left, right);

                if (Kind == DefinedOperators.LessThenOrEqual)
                    return Runtime.Integers.LessThenOrEqual(left, right);

                if (Kind == DefinedOperators.GreaterThenEqual)
                    return Runtime.Integers.GreaterThenEqual(left, right);

            }

            if (left is IStringValue && right is IStringValue) {

                if (Kind == DefinedOperators.EqualsOperator)
                    return Runtime.Strings.Equal(left, right);

                if (Kind == DefinedOperators.NotEqualsOperator)
                    return Runtime.Strings.NotEquals(left, right);

                if (Kind == DefinedOperators.LessThen)
                    return Runtime.Strings.LessThen(left, right);

                if (Kind == DefinedOperators.GreaterThen)
                    return Runtime.Strings.GreaterThen(left, right);

                if (Kind == DefinedOperators.LessThenOrEqual)
                    return Runtime.Strings.LessThenOrEqual(left, right);

                if (Kind == DefinedOperators.GreaterThenEqual)
                    return Runtime.Strings.GreaterThenEqual(left, right);

            }

            return null;
        }
    }
}
