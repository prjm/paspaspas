using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     weak link rtti switch
    /// </summary>
    public class WeakLinkRtti : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public RttiLinkMode Mode { get; set; }
    }
}
