using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     symbol definitions switch
    /// </summary>
    public class SymbolDefinitions : SyntaxPartBase {

        /// <summary>
        ///     definition mode
        /// </summary>
        public SymbolDefinitionInfo Mode { get; set; }

        /// <summary>
        ///     references mode
        /// </summary>
        public SymbolReferenceInfo ReferencesMode { get; set; }
    }
}
