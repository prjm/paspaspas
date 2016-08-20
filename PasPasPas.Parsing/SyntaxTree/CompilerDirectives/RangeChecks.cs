using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     range checks switch
    /// </summary>
    public class RangeChecks : SyntaxPartBase {

        /// <summary>
        ///     range check mode
        /// </summary>
        public RuntimeRangeChecks Mode { get; set; }
    }
}
