using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     implementation for a combined symbol table
    /// </summary>
    /// <typeparam name="TItems"></typeparam>
    /// <typeparam name="TSymbol"></typeparam>
    public abstract class CombinedSymbolTableBase<TItems, TSymbol> : SymbolTableBase<TSymbol>
        where TItems : AbstractSyntaxPartBase
        where TSymbol : AbstractSyntaxPartBase, ISymbolTableEntry {

        /// <summary>
        ///     list of items
        /// </summary>
        public ISyntaxPartList<TItems> Items { get; }

        /// <summary>
        ///     creates a new combined symbol table
        /// </summary>
        public CombinedSymbolTableBase()
            => Items = new SyntaxPartCollection<TItems>(this);

    }
}
