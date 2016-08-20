using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     warnings directive
    /// </summary>
    public class Warnings : SyntaxPartBase {

        /// <summary>
        ///     warning mode
        /// </summary>
        public CompilerWarnings Mode { get; set; }
    }
}
