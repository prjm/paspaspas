using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     marks a symbol with attributes
    /// </summary>
    public interface ISymbolWithAttributes {

        /// <summary>
        ///     availiable attributes
        /// </summary>
        IEnumerable<SymbolAttribute> Attributes { get; set; }

    }
}