﻿using System.Collections.Generic;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a list of declared symbols
    /// </summary>
    public class DeclaredSymbols : CombinedSymbolTableBase<DeclaredSymbolGroup, DeclaredSymbol>, IDeclaredSymbolTarget {

        /// <summary>
        ///     direct items
        /// </summary>
        public IList<DeclaredSymbol> DirectItems { get; }

        /// <summary>
        ///     create a new set of declared symbols
        /// </summary>
        public DeclaredSymbols() =>
            DirectItems = new SyntaxPartCollection<DeclaredSymbol>(this);

        /// <summary>
        ///     log duplicated unit name
        /// </summary>
        /// <param name="newDuplicate"></param>
        /// <param name="logSource"></param>
        protected override void LogDuplicateSymbolError(DeclaredSymbol newDuplicate, LogSource logSource) {
            base.LogDuplicateSymbolError(newDuplicate, logSource);
            logSource.Error(StructuralErrors.RedeclaredSymbol, newDuplicate);
        }

        /// <summary>
        ///     enumerate all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (AbstractSyntaxPartBase part in Items)
                    yield return part;
                foreach (DeclaredSymbol part in DirectItems)
                    yield return part;
            }
        }

        /// <summary>
        ///     declared symbols
        /// </summary>
        public DeclaredSymbols Symbols
            => this;

        /// <summary>
        ///     add symbols
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="logSource"></param>
        public void AddDirect(DeclaredSymbol entry, LogSource logSource) {
            if (Add(entry, logSource)) {
                DirectItems.Add(entry);
            }
        }

        /// <summary>
        ///     find a declared symbol
        /// </summary>
        /// <param name="name">symbol to find</param>
        /// <returns></returns>
        public DeclaredSymbol Find(IEnumerable<GenericSymbolNamePart> name) {
            DeclaredSymbols symbols = this;
            DeclaredSymbol symbol = null;

            foreach (GenericSymbolNamePart part in name) {

                if (symbols == null)
                    return null;

                if (!symbols.Contains(part.Name))
                    return null;

                symbol = symbols[part.Name];

                if (symbol == null)
                    return null;

                symbols = (symbol as IDeclaredSymbolTarget)?.Symbols;
            }

            return symbol;
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
