using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     hpp emit
    /// </summary>
    public class HppEmit : SyntaxPartBase {

        /// <summary>
        ///     value to emit
        /// </summary>
        public string EmitValue { get; set; }

        /// <summary>
        ///     emit mode
        /// </summary>
        public HppEmitMode Mode { get; set; }
    }
}
