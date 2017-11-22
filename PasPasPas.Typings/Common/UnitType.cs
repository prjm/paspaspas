using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     unit type definition
    /// </summary>
    public class UnitType : TypeBase {

        private IDictionary<string, ScopeEntry> symbols
            = new Dictionary<string, ScopeEntry>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     unit type
        /// </summary>
        /// <param name="withId"></param>
        public UnitType(int withId) : base(withId) {
        }

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.Unit;

        /// <summary>
        ///     register a symbol
        /// </summary>
        /// <param name="symbolName">symbol name</param>
        /// <param name="entry">defined entry</param>
        public void RegisterSymbol(string symbolName, ScopeEntry entry)
            => symbols.Add(symbolName, entry);

        /// <summary>
        ///     resolve a symbol in this unit
        /// </summary>
        /// <param name="symbolName">symbol to resolve</param>
        /// <param name="entry">resolved symbol</param>
        /// <returns><c>true</c> if the symbol was resolved</returns>
        public bool TryToResolve(string symbolName, out ScopeEntry entry)
            => symbols.TryGetValue(symbolName, out entry);
    }
}
