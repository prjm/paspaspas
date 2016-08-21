using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     strong link types directive
    /// </summary>
    public class StrongLinkTypes : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public StrongTypeLinking Mode { get; set; }
    }
}
