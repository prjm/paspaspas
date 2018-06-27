using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembler block
    /// </summary>
    public class AsmBlockSymbol : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     create a new asm block symbol
        /// </summary>
        /// <param name="asmSymbol"></param>
        /// <param name="endSymbol"></param>
        /// <param name="items"></param>
        public AsmBlockSymbol(Terminal asmSymbol, Terminal endSymbol, ImmutableArray<SyntaxPartBase> items) : base(items) {
            AsmSymbol = asmSymbol;
            EndSymbol = endSymbol;
        }

        /// <summary>
        ///     asm terminal
        /// </summary>
        public Terminal AsmSymbol { get; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }

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
        public override int Length
            => AsmSymbol.GetSymbolLength() +
               ItemLength +
               EndSymbol.GetSymbolLength();

    }
}
