using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     rrti control directive
    /// </summary>
    public class RttiControl : SyntaxPartBase {
        public RttiForVisibility Fields { get; internal set; }
        public RttiForVisibility Methods { get; internal set; }

        /// <summary>
        ///     selected rtti mode
        /// </summary>
        public RttiGenerationMode Mode { get; internal set; }
        public RttiForVisibility Properties { get; internal set; }
    }
}
