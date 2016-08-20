using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     stack frames directive
    /// </summary>
    public class StackFrames : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public StackFrameGeneration Mode { get; internal set; }
    }
}
