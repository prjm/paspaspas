using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     code align parameter
    /// </summary>
    public class CodeAlignParameter : SyntaxPartBase {

        /// <summary>
        ///     code align mode
        /// </summary>
        public CodeAlignment CodeAlign { get; set; }
    }
}
