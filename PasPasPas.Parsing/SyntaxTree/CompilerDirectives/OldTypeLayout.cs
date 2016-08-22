using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     old type layout directive
    /// </summary>
    public class OldTypeLayout : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public OldRecordTypes Mode { get; set; }
    }
}
