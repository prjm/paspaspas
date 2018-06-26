using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembler block
    /// </summary>
    public class AsmBlockSymbol : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     create a new asm block symbol
        /// </summary>
        /// <param name="items"></param>
        public AsmBlockSymbol(ImmutableArray<SyntaxPartBase> items) : base(items) {

        }

        /// <summary>
        ///     asm terminal
        /// </summary>
        public Terminal AsmSymbol { get; set; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, AsmSymbol, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => AsmSymbol.Length + ItemLength + EndSymbol.Length;

    }
}
