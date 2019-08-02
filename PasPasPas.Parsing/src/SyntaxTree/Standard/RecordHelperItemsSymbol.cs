using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record helper items
    /// </summary>
    public class RecordHelperItemsSymbol : VariableLengthSyntaxTreeBase<RecordHelperItemSymbol> {

        /// <summary>
        ///     create a new set of record helper items
        /// </summary>
        /// <param name="items"></param>
        public RecordHelperItemsSymbol(ImmutableArray<RecordHelperItemSymbol> items) : base(items) {
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