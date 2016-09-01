using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     rrti control directive
    /// </summary>
    public class RttiControl : SyntaxPartBase {

        /// <summary>
        ///     fields visibility
        /// </summary>
        public RttiForVisibility Fields { get; set; }

        /// <summary>
        ///     methods visibilty
        /// </summary>
        public RttiForVisibility Methods { get; set; }

        /// <summary>
        ///     properties visibility
        /// </summary>
        public RttiForVisibility Properties { get; internal set; }


        /// <summary>
        ///     selected rtti mode
        /// </summary>
        public RttiGenerationMode Mode { get; internal set; }
    }
}
