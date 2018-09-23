using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespace file name list
    /// </summary>
    public class NamespaceFileNameListSymbol : VariableLengthSyntaxTreeBase<NamespaceFileNameSymbol> {

        /// <summary>
        ///     create a new namespace file name list
        /// </summary>
        /// <param name="items"></param>
        public NamespaceFileNameListSymbol(ImmutableArray<NamespaceFileNameSymbol> items) : base(items) { }

        /// <summary>
        ///     accept visitor{}
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength;

    }
}