using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     method info directive
    /// </summary>
    public class MethodInfo : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public MethodInfoRtti Mode { get; set; }
    }
}
