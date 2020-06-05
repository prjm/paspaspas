#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record items
    /// </summary>
    public class RecordItemsSymbol : VariableLengthSyntaxTreeBase<RecordItemSymbol> {

        /// <summary>
        ///     create a new record items list
        /// </summary>
        /// <param name="items"></param>
        public RecordItemsSymbol(ImmutableArray<RecordItemSymbol> items) : base(items) {
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