using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     run only directive
    /// </summary>
    public class RunOnly : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public RuntimePackageMode Mode { get; set; }
    }
}
