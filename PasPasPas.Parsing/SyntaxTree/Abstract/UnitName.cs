namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a unit name
    /// </summary>
    public class UnitName : SymbolTableEntryBase {


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
        public override string SymbolName
        {
            get
            {
                if (Name != null)
                    return Name.CompleteName;
                else
                    return "Unit_" + GetHashCode();
            }
        }

    }
}