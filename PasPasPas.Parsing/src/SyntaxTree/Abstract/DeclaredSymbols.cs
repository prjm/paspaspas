using System.Collections.Generic;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a list of declared symbols
    /// </summary>
    public class DeclaredSymbolCollection : CombinedSymbolTableBaseCollection<DeclaredSymbolGroup, DeclaredSymbol>, IDeclaredSymbolTarget {

        /// <summary>
        ///     log duplicated unit name
        /// </summary>
        /// <param name="newDuplicate"></param>
        /// <param name="logSource"></param>
        protected override void LogDuplicateSymbolError(DeclaredSymbol newDuplicate, ILogSource logSource) {
            base.LogDuplicateSymbolError(newDuplicate, logSource);
            logSource.LogError(StructuralErrors.RedeclaredSymbol, newDuplicate);
        }

        /// <summary>
        ///     declared symbols
        /// </summary>
        public DeclaredSymbolCollection Symbols
            => this;

        /// <summary>
        ///     find a declared symbol
        /// </summary>
        /// <param name="name">symbol to find</param>
        /// <returns></returns>
        public DeclaredSymbol Find(IEnumerable<GenericSymbolNamePart> name) {
            var symbols = this;
            DeclaredSymbol symbol = null;

            foreach (var part in name) {

                if (symbols == null)
                    return null;

                var partName = part.Name;

                if (part.Parameters != default && part.Parameters.Count > 0)
                    partName = string.Concat(partName, AbstractSyntaxPartBase.GenericSeparator, part.Parameters.Count);

                if (!symbols.Contains(partName))
                    return null;

                symbol = symbols[partName];

                if (symbol == null)
                    return null;

                symbols = (symbol as IDeclaredSymbolTarget)?.Symbols;
            }

            return symbol;
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Items, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     returns always <c>true</c>
        /// </summary>
        protected override bool HasDuplicateReplacement
            => true;

        /// <summary>
        ///     try to merge duplicate declarations
        /// </summary>
        /// <param name="existingEntry"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected override DeclaredSymbol MergeDuplicates(DeclaredSymbol existingEntry, DeclaredSymbol entry) {
            var methodGroup = existingEntry as MethodGroup;
            var newEntry = entry as IMethodImplementation;
            var oldEntry = existingEntry as IMethodImplementation;

            if (methodGroup != default && newEntry != default && methodGroup.TryToAdd(newEntry)) {
                newEntry.Anchor = default;
                return methodGroup;
            }

            else if (oldEntry != default && newEntry != default) {
                methodGroup = new MethodGroup();
                methodGroup.Add(oldEntry);

                var anchor = oldEntry.Anchor;
                anchor.Symbol = methodGroup;
                oldEntry.Anchor = default;
                newEntry.Anchor = default;

                if (methodGroup.TryToAdd(newEntry))
                    return methodGroup;
            }

            return default;
        }
    }
}
