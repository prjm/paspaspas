﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <returns></returns>
        public override int GetOutputTypeForOperation(Signature input) {
            if (input.Length != 2)
                return TypeIds.ErrorType;

            var left = TypeRegistry.GetTypeKind(input[0]);
            var right = TypeRegistry.GetTypeKind(input[1]);

            if (CommonTypeKind.BooleanType.All(left, right))
                return TypeIds.BooleanType;

            if (left.Numerical() && right.Numerical())
                return TypeIds.BooleanType;

            if (left.Textual() && right.Textual())
                return TypeIds.BooleanType;


            return TypeIds.ErrorType;
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
    }
}