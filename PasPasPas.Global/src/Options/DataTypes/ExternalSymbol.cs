namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     external symbol information
    /// </summary>
    public class ExternalSymbol {

        /// <summary>
        ///     create a new external symbol reference
        /// </summary>
        /// <param name="identifierName"></param>
        /// <param name="symbolName"></param>
        /// <param name="unionName"></param>
        public ExternalSymbol(string identifierName, string symbolName, string unionName) {
            IdentifierName = identifierName;
            SymbolName = symbolName;
            UnionName = unionName;
        }

        /// <summary>
        ///     identifier name
        /// </summary>
        public string IdentifierName { get; }

        /// <summary>
        ///     simple symbol name
        /// </summary>
        public string SymbolName { get; }

        /// <summary>
        ///     name in unions
        /// </summary>
        public string UnionName { get; }
    }
}