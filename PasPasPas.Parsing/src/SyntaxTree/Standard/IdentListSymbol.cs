using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     comma separated list of identifiers
    /// </summary>
    public class IdentifierListSymbol : VariableLengthSyntaxTreeBase<IdentifierListItem> {

        /// <summary>
        ///     create a new identifier list
        /// </summary>
        /// <param name="items"></param>
        public IdentifierListSymbol(ImmutableArray<IdentifierListItem> items) : base(items) {
        }

        /// <summary>
        ///     accept visitor
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