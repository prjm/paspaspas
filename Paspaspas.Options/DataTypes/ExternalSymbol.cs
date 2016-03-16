namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     external symbol information
    /// </summary>
    public class ExternalSymbol {

        /// <summary>
        ///     identifier name
        /// </summary>
        public string IdentifierName { get; internal set; }

        /// <summary>
        ///     simple symbol name
        /// </summary>
        public string SymbolName { get; internal set; }

        /// <summary>
        ///     name in unions
        /// </summary>
        public string UnionName { get; internal set; }
    }
}