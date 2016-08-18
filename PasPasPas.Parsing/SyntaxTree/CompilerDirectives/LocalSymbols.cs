using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     local symbols directive
    /// </summary>
    public class LocalSymbols : SyntaxPartBase {

        /// <summary>
        ///     symbol mode
        /// </summary>
        public LocalDebugSymbols Mode { get; set; }
    }
}
