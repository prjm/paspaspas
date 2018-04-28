using System;
using System.Collections.Generic;
using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     virtual unit type definition
    /// </summary>
    public class UnitType : TypeBase {

        /// <summary>
        ///     declared symbols
        /// </summary>
        private IDictionary<string, Reference> symbols
            = new Dictionary<string, Reference>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     global routines
        /// </summary>
        private IList<IRoutine> globalRoutines
            = new List<IRoutine>();

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
        public void RegisterSymbol(string symbolName, Reference entry)
            => symbols.Add(symbolName, entry);

        /// <summary>
        ///     resolve a symbol in this unit
        /// </summary>
        /// <param name="symbolName">symbol to resolve</param>
        /// <param name="entry">resolved symbol</param>
        /// <returns><c>true</c> if the symbol was resolved</returns>
        public bool TryToResolve(string symbolName, out Reference entry)
            => symbols.TryGetValue(symbolName, out entry);

        /// <summary>
        ///     add a global routine
        /// </summary>
        /// <param name="routine"></param>
        public void AddGlobal(IRoutine routine) {
            globalRoutines.Add(routine);
            symbols.Add(routine.Name, new Reference(ReferenceKind.RefToGlobalRoutine, routine));
        }
    }
}
