using System.Collections.Generic;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a list of declared symbols
    /// </summary>
    public class DeclaredSymbols : CombinedSymbolTableBase<DeclaredSymbolGroup, DeclaredSymbol>, IDeclaredSymbolTarget {

        /// <summary>
        ///     direct items
        /// </summary>
        public IList<DeclaredSymbol> DirectItems { get; } =
            new List<DeclaredSymbol>();

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
                foreach (AbstractSyntaxPart part in Items)
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
    }
}
