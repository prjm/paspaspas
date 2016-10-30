namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for symbol table entries
    /// </summary>
    public abstract class SymbolTableEntryBase : AbstractSyntaxPart, ISymbolTableEntry {

        /// <summary>
        ///     name of the symbol
        /// </summary>
        public string SymbolName
            => InternalSymbolName ?? "Undefined_" + GetType().FullName + "_" + GetHashCode();

        /// <summary>
        ///     overridable symbol name
        /// </summary>
        protected abstract string InternalSymbolName { get; }

    }
}