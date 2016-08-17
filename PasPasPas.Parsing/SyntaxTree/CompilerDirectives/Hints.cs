using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     hints directive
    /// </summary>
    public class Hints : SyntaxPartBase {

        /// <summary>
        ///     hint mode
        /// </summary>
        public CompilerHints Mode { get; set; }
    }
}
