using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     open strings switch
    /// </summary>
    public class OpenStrings : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public OpenStringTypes Mode { get; internal set; }
    }
}
