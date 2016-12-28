using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     io checks directive
    /// </summary>
    public class IoChecks : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public IoCallCheck Mode { get; set; }
    }
}
