using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     switch to deny units in packages
    /// </summary>
    public class ParseDenyPackageUnit : SyntaxPartBase {

        /// <summary>
        ///     switch value
        /// </summary>
        public DenyUnitInPackages DenyUnit { get; set; }
    }
}
