using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Infrastructure.Common {

    /// <summary>
    ///     exception on constant propagation
    /// </summary>
    public class InvalidArithmeticOperationException : InvalidOperationException {

        private string operationName;
        private object operationValue;

        /// <summary>
        ///     operation value
        /// </summary>
        /// <param name="opName"></param>
        /// <param name="opValue"></param>
        public InvalidArithmeticOperationException(string opName, object opValue) {
            operationName = opName;
            operationValue = opValue;
        }
    }
}
