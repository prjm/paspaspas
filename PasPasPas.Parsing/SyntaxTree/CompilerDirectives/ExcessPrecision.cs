using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     excess preicision directive
    /// </summary>
    public class ExcessPrecision : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public ExcessPrecisionForResults Mode { get; set; }
    }
}
