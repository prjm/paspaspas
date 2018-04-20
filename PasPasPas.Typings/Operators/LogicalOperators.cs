using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     <c>not</c> operation
    /// </summary>
    public class LogicalOperators : OperatorBase {

        private static void Register(ITypeRegistry registry, int kind, int arity = 2)
            => registry.RegisterOperator(new LogicalOperators(kind, arity));

        /// <summary>
        ///     register logical operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, DefinedOperators.NotOperation, 1);
            Register(registry, DefinedOperators.AndOperation);
            Register(registry, DefinedOperators.XorOperation);
            Register(registry, DefinedOperators.OrOperation);
            Register(registry, DefinedOperators.ShlOperation);
            Register(registry, DefinedOperators.ShrOperation);
        }

        /// <summary>
        ///     create a new logical operation
        /// </summary>
        /// <param name="withKind">operator kind</param>
        /// <param name="withArity">operator arity</param>
        public LogicalOperators(int withKind, int withArity) : base(withKind, withArity) { }

        /// <summary>
        ///     operation name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.AndOperation:
                        return "and";
                    case DefinedOperators.OrOperation:
                        return "or";
                    case DefinedOperators.XorOperation:
                        return "xor";
                    case DefinedOperators.NotOperation:
                        return "not";
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     compute the output type of this operation
        /// </summary>
        /// <param name="input">input signature</param>
        /// <param name="values">current values</param>
        /// <returns>output type</returns>
        public override int GetOutputTypeForOperation(Signature input, object[] values) {

            if (input.Length == 1) {

                var operand = TypeRegistry.GetTypeKind(input[0]);

                if (Kind == DefinedOperators.NotOperation && operand == CommonTypeKind.BooleanType)
                    return KnownTypeIds.BooleanType;

                if (Kind == DefinedOperators.NotOperation && operand == CommonTypeKind.Int64Type)
                    return input[0];

                if (Kind == DefinedOperators.NotOperation && operand == CommonTypeKind.IntegerType)
                    return input[0];

            }
            else if (input.Length == 2) {

                var left = TypeRegistry.GetTypeKind(input[0]);
                var right = TypeRegistry.GetTypeKind(input[1]);

                if (Kind == DefinedOperators.AndOperation && CommonTypeKind.BooleanType.All(left, right))
                    return KnownTypeIds.BooleanType;

                if (Kind == DefinedOperators.AndOperation && left.Integral() && right.Integral())
                    return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0], input[1]);

                if (Kind == DefinedOperators.OrOperation && CommonTypeKind.BooleanType.All(left, right))
                    return KnownTypeIds.BooleanType;

                if (Kind == DefinedOperators.OrOperation && left.Integral() && right.Integral())
                    return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0], input[1]);

                if (Kind == DefinedOperators.XorOperation && CommonTypeKind.BooleanType.All(left, right))
                    return KnownTypeIds.BooleanType;

                if (Kind == DefinedOperators.XorOperation && left.Integral() && right.Integral())
                    return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0], input[1]);

                if (Kind == DefinedOperators.ShlOperation && left.Integral() && right.Integral())
                    return input[0];

                if (Kind == DefinedOperators.ShrOperation && left.Integral() && right.Integral())
                    return input[0];

            }

            return KnownTypeIds.ErrorType;
        }

        /// <summary>
        ///     compute the value
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

            if (Kind == DefinedOperators.NotOperation) {

                if (value is IIntegerValue intValue)
                    return Runtime.Integers.Not(value);

                if (value is IBooleanValue boolValue)
                    return Runtime.Booleans.Not(value);

            }

            return null;
        }

        private IValue ComputeBinaryOperator(IValue value1, IValue value2) {

            if (value1 is IIntegerValue int1 && value2 is IIntegerValue int2) {

                if (Kind == DefinedOperators.AndOperation) {
                    return Runtime.Integers.And(value1, value2);
                }

                if (Kind == DefinedOperators.OrOperation) {
                    return Runtime.Integers.Or(value1, value2);
                }

                if (Kind == DefinedOperators.XorOperation) {
                    return Runtime.Integers.Xor(value1, value2);
                }

                if (Kind == DefinedOperators.ShlOperation) {
                    return Runtime.Integers.Shl(value1, value2);
                }

                if (Kind == DefinedOperators.ShrOperation) {
                    return Runtime.Integers.Shr(value1, value2);
                }


            }

            if (value1 is IBooleanValue bool1 && value2 is IBooleanValue bool2) {

                if (Kind == DefinedOperators.AndOperation) {
                    return Runtime.Booleans.And(value1, value2);
                }

                if (Kind == DefinedOperators.OrOperation) {
                    return Runtime.Booleans.Or(value1, value2);
                }

                if (Kind == DefinedOperators.XorOperation) {
                    return Runtime.Booleans.Xor(value1, value2);
                }

            }

            return null;


        }
    }
}