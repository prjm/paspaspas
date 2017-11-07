﻿using System;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     <c>not</c> operation
    /// </summary>
    public class LogicalOperators : OperatorBase {

        /// <summary>
        ///     create a new logical operation
        /// </summary>
        /// <param name="withKind">operation kind</param>
        public LogicalOperators(int withKind) : base(withKind) { }

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
        /// <returns>output type</returns>
        public override int GetOutputTypeForOperation(Signature input) {

            if (input.Length == 1) {

                var operand = TypeRegistry.GetTypeKind(input[0]);

                if (Kind == DefinedOperators.NotOperation && operand == CommonTypeKind.BooleanType)
                    return TypeIds.BooleanType;


                if (Kind == DefinedOperators.NotOperation && operand == CommonTypeKind.Int64Type)
                    return input[0];

                if (Kind == DefinedOperators.NotOperation && operand == CommonTypeKind.IntegerType)
                    return input[0];

            }
            else if (input.Length == 2) {

                var left = TypeRegistry.GetTypeKind(input[0]);
                var right = TypeRegistry.GetTypeKind(input[1]);

                if (Kind == DefinedOperators.AndOperation && CommonTypeKind.BooleanType.All(left, right))
                    return TypeIds.BooleanType;

                if (Kind == DefinedOperators.AndOperation && left.Integral() && right.Integral())
                    return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0], input[1]);

                if (Kind == DefinedOperators.OrOperation && CommonTypeKind.BooleanType.All(left, right))
                    return TypeIds.BooleanType;

                if (Kind == DefinedOperators.OrOperation && left.Integral() && right.Integral())
                    return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0], input[1]);

                if (Kind == DefinedOperators.XorOperation && CommonTypeKind.BooleanType.All(left, right))
                    return TypeIds.BooleanType;

                if (Kind == DefinedOperators.XorOperation && left.Integral() && right.Integral())
                    return TypeRegistry.GetSmallestIntegralTypeOrNext(input[0], input[1]);

                if (Kind == DefinedOperators.ShlOperation && left.Integral() && right.Integral())
                    return input[0];

                if (Kind == DefinedOperators.ShrOperation && left.Integral() && right.Integral())
                    return input[0];

            }

            return TypeIds.ErrorType;
        }

        /// <summary>
        ///     register logical operators
        /// </summary>
        /// <param name="registry">type registry</param>
        public static void RegisterOperators(ITypeRegistry registry) {
            registry.RegisterOperator(new LogicalOperators(DefinedOperators.NotOperation));
            registry.RegisterOperator(new LogicalOperators(DefinedOperators.AndOperation));
            registry.RegisterOperator(new LogicalOperators(DefinedOperators.XorOperation));
            registry.RegisterOperator(new LogicalOperators(DefinedOperators.OrOperation));
            registry.RegisterOperator(new LogicalOperators(DefinedOperators.ShlOperation));
            registry.RegisterOperator(new LogicalOperators(DefinedOperators.ShrOperation));
        }
    }
}
