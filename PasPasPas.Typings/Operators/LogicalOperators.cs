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

        /// <summary>
        ///     unary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override int EvaluateUnaryOperator(Signature input) {
            var operand = TypeRegistry.GetTypeKind(input[0].TypeId);

            if (Kind == DefinedOperators.NotOperation && operand == CommonTypeKind.BooleanType)
                return KnownTypeIds.BooleanType;

            if (Kind == DefinedOperators.NotOperation && operand == CommonTypeKind.Int64Type)
                return input[0].TypeId;

            if (Kind == DefinedOperators.NotOperation && operand == CommonTypeKind.IntegerType)
                return input[0].TypeId;

            return KnownTypeIds.ErrorType;
        }

        /// <summary>
        ///     binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override int EvaluateBinaryOperator(Signature input) {
            var left = TypeRegistry.GetTypeKind(input[0].TypeId);
            var right = TypeRegistry.GetTypeKind(input[1].TypeId);

            if (Kind == DefinedOperators.AndOperation && CommonTypeKind.BooleanType.All(left, right))
                return KnownTypeIds.BooleanType;

            if (Kind == DefinedOperators.AndOperation && left.Integral() && right.Integral())
                return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0].TypeId, input[1].TypeId);

            if (Kind == DefinedOperators.OrOperation && CommonTypeKind.BooleanType.All(left, right))
                return KnownTypeIds.BooleanType;

            if (Kind == DefinedOperators.OrOperation && left.Integral() && right.Integral())
                return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0].TypeId, input[1].TypeId);

            if (Kind == DefinedOperators.XorOperation && CommonTypeKind.BooleanType.All(left, right))
                return KnownTypeIds.BooleanType;

            if (Kind == DefinedOperators.XorOperation && left.Integral() && right.Integral())
                return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0].TypeId, input[1].TypeId);

            if (Kind == DefinedOperators.ShlOperation && left.Integral() && right.Integral())
                return input[0].TypeId;

            if (Kind == DefinedOperators.ShrOperation && left.Integral() && right.Integral())
                return input[0].TypeId;

            return KnownTypeIds.ErrorType;
        }
    }
}