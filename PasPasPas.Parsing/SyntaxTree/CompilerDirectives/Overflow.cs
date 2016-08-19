using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     overflow check switch
    /// </summary>
    public class Overflow : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public RuntimeOverflowChecks Mode { get; set; }
    }
}
