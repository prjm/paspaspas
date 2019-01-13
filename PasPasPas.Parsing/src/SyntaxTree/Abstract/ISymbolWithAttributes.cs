using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     marks a symbol with attributes
    /// </summary>
    public interface ISymbolWithAttributes {

        /// <summary>
        ///     available attributes
        /// </summary>
        [System.Obsolete("Replacement required")]
        List<SymbolAttributeItem> Attributes { get; }

    }
}