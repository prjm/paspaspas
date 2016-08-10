using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     application type parameter
    /// </summary>
    public class AppTypeParameter : SyntaxPartBase {

        /// <summary>
        ///     application type
        /// </summary>
        public AppType ApplicationType { get; set; }
    }
}
