
using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     safe divide switch
    /// </summary>
    public class SafeDivide : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public FDivSafeDivide Mode { get; internal set; }
    }
}
