using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     weak package unit directive
    /// </summary>
    public class WeakPackageUnit : SyntaxPartBase {

        /// <summary>
        ///     packaging mode
        /// </summary>
        public WeakPackaging Mode { get; set; }
    }
}
