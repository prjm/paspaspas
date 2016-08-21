using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     legacy if end directive
    /// </summary>
    public class LegacyIfEnd : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public EndIfMode Mode { get; set; }
    }
}
