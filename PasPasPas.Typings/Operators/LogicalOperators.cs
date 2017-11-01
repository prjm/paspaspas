using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operations {

    /// <summary>
    ///     <c>not</c> operation
    /// </summary>
    public class LogicalOperators : IOperator {

        private int kind;

        /// <summary>
        ///     create a new logical operation
        /// </summary>
        /// <param name="withKind">operation kind</param>
        public LogicalOperators(int withKind)
            => kind = withKind;

        /// <summary>
        ///     operation kind - <c>DefinedOperations.NotOperation</c>
        /// </summary>
        public int Kind
            => kind;

        /// <summary>
        ///     operation name
        /// </summary>
        public string Name {
            get {
                switch (kind) {
                    case DefinedOperators.AndOperation:
                        return "and";
                    case DefinedOperators.OrOperation:
                        return "or";
                    case DefinedOperators.XorOperation:
                        return "xor";
                    case DefinedOperators.NotOperation:
                        return "not";
                }
                return null;
            }
        }

        /// <summary>
        ///     compute the output type of this operation
        /// </summary>
        /// <param name="input">input signature</param>
        /// <returns>output type</returns>
        public int GetOutputTypeForOperation(Signature input) {
            if (kind == DefinedOperators.NotOperation && input.EqualsType(TypeIds.BooleanType))
                return TypeIds.BooleanType;

            if (kind == DefinedOperators.AndOperation && input.EqualsType(TypeIds.BooleanType, TypeIds.BooleanType))
                return TypeIds.BooleanType;

            if (kind == DefinedOperators.OrOperation && input.EqualsType(TypeIds.BooleanType, TypeIds.BooleanType))
                return TypeIds.BooleanType;

            if (kind == DefinedOperators.XorOperation && input.EqualsType(TypeIds.BooleanType, TypeIds.BooleanType))
                return TypeIds.BooleanType;


            return TypeIds.ErrorType;
        }
    }
}
