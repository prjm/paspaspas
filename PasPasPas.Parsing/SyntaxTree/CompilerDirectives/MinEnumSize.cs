using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     enum size directive
    /// </summary>
    public class MinEnumSize : SyntaxPartBase {

        /// <summary>
        ///     enum size
        /// </summary>
        public EnumSize Size { get; set; }
    }
}
