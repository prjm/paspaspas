using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     ifopt directive
    /// </summary>
    public class IfOpt : SyntaxPartBase {

        /// <summary>
        ///     required kind
        /// </summary>
        public string SwitchKind { get; set; }

        /// <summary>
        ///     required info
        /// </summary>
        public SwitchInfo SwitchInfo { get; set; }

        /// <summary>
        ///     required state
        /// </summary>
        public SwitchInfo SwitchState { get; set; }
    }
}
