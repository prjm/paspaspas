using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     implicit build directive
    /// </summary>
    public class ImplicitBuild : SyntaxPartBase {

        /// <summary>
        ///     implicit build mode
        /// </summary>
        public ImplicitBuildUnit Mode { get; set; }
    }
}
