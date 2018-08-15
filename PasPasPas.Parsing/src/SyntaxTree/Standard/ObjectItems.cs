using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     object items
    /// </summary>
    public class ObjectItems : VariableLengthSyntaxTreeBase<ObjectItem> {

        /// <summary>
        ///     create a new object item
        /// </summary>
        /// <param name="items"></param>
        public ObjectItems(ImmutableArray<ObjectItem> items) : base(items) { }

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
        public override int Length => ItemLength;

    }
}