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
    public class NotOperation : IOperation {

        /// <summary>
        ///     operation kind - <c>DefinedOperations.NotOperation</c>
        /// </summary>
        public int Kind
            => DefinedOperations.NotOperation;

        /// <summary>
        ///     operation name
        /// </summary>
        public string Name
            => "not";

        /// <summary>
        ///     compute the output type of this operation
        /// </summary>
        /// <param name="input">input signature</param>
        /// <returns>output type</returns>
        public int GetOutputTypeForOperation(Signature input) {
            if (input.EqualsType(TypeIds.BooleanType))
                return TypeIds.BooleanType;

            return TypeIds.ErrorType;
        }
    }
}
