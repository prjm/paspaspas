using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     pointer math directive
    /// </summary>
    public class PointerMath : SyntaxPartBase {

        /// <summary>
        ///     pointer math mode
        /// </summary>
        public PointerManipulation Mode { get; set; }
    }
}
