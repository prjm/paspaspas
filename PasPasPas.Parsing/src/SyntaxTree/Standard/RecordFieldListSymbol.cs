using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record field list
    /// </summary>
    public class RecordFieldListSymbol : VariableLengthSyntaxTreeBase<RecordFieldSymbol> {

        /// <summary>
        ///     create a new record field list
        /// </summary>
        /// <param name="items"></param>
        public RecordFieldListSymbol(ImmutableArray<RecordFieldSymbol> items) : base(items) {
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