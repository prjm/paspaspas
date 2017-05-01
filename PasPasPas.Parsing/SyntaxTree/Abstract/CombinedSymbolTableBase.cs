using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     implementation for a combined symbol table
    /// </summary>
    /// <typeparam name="TItems"></typeparam>
    /// <typeparam name="TSymbol"></typeparam>
    public abstract class CombinedSymbolTableBase<TItems, TSymbol> : SymbolTableBase<TSymbol>
        where TItems : AbstractSyntaxPart
        where TSymbol : AbstractSyntaxPart, ISymbolTableEntry {

        /// <summary>
        ///     list of items
        /// </summary>
        public IList<TItems> Items { get; }
            = new List<TItems>();

    }
}
