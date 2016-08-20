using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     writable constants
    /// </summary>
    public class WritableConsts : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public ConstantValues Mode { get; set; }
    }
}
