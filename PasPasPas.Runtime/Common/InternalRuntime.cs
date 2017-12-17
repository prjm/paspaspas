using PasPasPas.Infrastructure.Common;
using PasPasPas.Runtime.Operators;

namespace PasPasPas.Runtime.Common {

    /// <summary>
    ///     internal runtime implementation
    /// </summary>
    public class InternalRuntime : IRuntime {

        /// <summary>
        ///     constant helper
        /// </summary>
        public IConstantOperations Constants { get; }
            = new OperatorsOnConstants();
    }
}
