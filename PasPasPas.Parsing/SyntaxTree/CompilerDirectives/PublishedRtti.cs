using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     typeinfo directive
    /// </summary>
    public class PublishedRtti : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public RttiForPublishedProperties Mode { get; set; }
    }
}
