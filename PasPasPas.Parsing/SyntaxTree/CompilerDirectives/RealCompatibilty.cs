using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     real48 compatibility directive
    /// </summary>
    public class RealCompatibility : SyntaxPartBase {

        /// <summary>
        ///     compatibility mode
        /// </summary>
        public Real48 Mode { get; set; }
    }
}
