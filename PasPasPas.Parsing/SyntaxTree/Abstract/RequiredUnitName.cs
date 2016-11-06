namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a unit name
    /// </summary>
    public class RequiredUnitName : SymbolTableEntryBase {


        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     unit mode
        /// </summary>
        public UnitMode Mode { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     file name of to the unit
        /// </summary>
        public string FileName { get; set; }
    }
}