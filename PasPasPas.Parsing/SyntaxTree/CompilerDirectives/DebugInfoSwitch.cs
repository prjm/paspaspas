using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     debug information swith
    /// </summary>
    public class DebugInfoSwitch : SyntaxPartBase {

        /// <summary>
        ///     debug information mode
        /// </summary>
        public DebugInformation DebugInfo { get; set; }
    }
}
