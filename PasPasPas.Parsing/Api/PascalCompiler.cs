using PasPasPas.Options.Bundles;

namespace PasPasPas.Api {

    /// <summary>
    ///     interface for a pascal compiler
    /// </summary>
    public class PascalCompiler {

        /// <summary>
        ///     comptiler options
        /// </summary>
        public CompilerOptions Options { get; }
            = new CompilerOptions();

        /// <summary>
        ///     description what the compiler should do
        /// </summary>
        public CompilerTask Task { get; }
            = new CompilerTask();


    }
}
