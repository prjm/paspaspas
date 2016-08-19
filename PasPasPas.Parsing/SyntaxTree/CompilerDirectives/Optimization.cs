using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     optimization switch
    /// </summary>
    public class Optimization : SyntaxPartBase {

        /// <summary>
        ///     optimization mode
        /// </summary>
        public CompilerOptmization Mode { get; set; }
    }
}
