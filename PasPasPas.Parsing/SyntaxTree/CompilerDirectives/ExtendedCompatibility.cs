using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     extended compatibility directive
    /// </summary>
    public class ExtendedCompatibility : SyntaxPartBase {

        /// <summary>
        ///     compatibility mode
        /// </summary>
        public ExtendedCompatiblityMode Mode { get; set; }
    }
}
