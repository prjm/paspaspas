using PasPasPas.Api.Options;

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


    }
}
