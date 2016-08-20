using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     warn switch
    /// </summary>
    public class WarnSwitch : SyntaxPartBase {

        /// <summary>
        ///     warning mode
        /// </summary>
        public WarningMode Mode { get; internal set; }

        /// <summary>
        ///     warning type
        /// </summary>
        public string WarningType { get; internal set; }
    }
}
