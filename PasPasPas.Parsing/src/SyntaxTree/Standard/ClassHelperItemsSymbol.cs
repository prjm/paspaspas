using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     format class helper items
    /// </summary>
    public class ClassHelperItemsSymbol : VariableLengthSyntaxTreeBase<ClassHelperItemSymbol> {

        /// <summary>
        ///     create a new class helper items symbol
        /// </summary>
        /// <param name="items"></param>
        public ClassHelperItemsSymbol(ImmutableArray<ClassHelperItemSymbol> items) : base(items) {

        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => ItemLength;

    }
}