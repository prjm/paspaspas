using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record items
    /// </summary>
    public class RecordItems : VariableLengthSyntaxTreeBase<RecordItem> {

        /// <summary>
        ///     create a new record items list
        /// </summary>
        /// <param name="items"></param>
        public RecordItems(ImmutableArray<RecordItem> items) : base(items) {
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