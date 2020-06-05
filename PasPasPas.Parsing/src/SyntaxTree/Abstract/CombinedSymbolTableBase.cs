#nullable disable
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     implementation for a combined symbol table
    /// </summary>
    /// <typeparam name="TItems"></typeparam>
    /// <typeparam name="TSymbol"></typeparam>
    public abstract class CombinedSymbolTableBaseCollection<TItems, TSymbol> : SymbolTableBaseCollection<TSymbol>
        where TItems : AbstractSyntaxPartBase
        where TSymbol : AbstractSyntaxPartBase, ISymbolTableEntry {

        /// <summary>
        ///     list of items
        /// </summary>
        public ISyntaxPartCollection<TItems> Items { get; }

        /// <summary>
        ///     creates a new combined symbol table
        /// </summary>
        protected CombinedSymbolTableBaseCollection()
            => Items = new SyntaxPartCollection<TItems>();

    }
}
