
using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     zero based strings directive
    /// </summary>
    public class ZeroBasedStrings : SyntaxPartBase {

        /// <summary>
        ///     string index mode
        /// </summary>
        public FirstCharIndex Mode { get; set; }
    }
}
