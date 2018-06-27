using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly factor
    /// </summary>
    public class AsmFactorSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     hex number
        /// </summary>
        public SyntaxPartBase HexNumber { get; internal set; }

        /// <summary>
        ///     identifier
        /// </summary>
        public SyntaxPartBase Identifier { get; internal set; }

        /// <summary>
        ///     memory subexpression
        /// </summary>
        public SyntaxPartBase MemorySubexpression { get; set; }

        /// <summary>
        ///     number
        /// </summary>
        public SyntaxPartBase Number { get; internal set; }

        /// <summary>
        ///     quoted string
        /// </summary>
        public SyntaxPartBase QuotedString { get; internal set; }

        /// <summary>
        ///     segment subexpression
        /// </summary>
        public SyntaxPartBase SegmentExpression { get; set; }

        /// <summary>
        ///     segment prefix
        /// </summary>
        public SyntaxPartBase SegmentPrefix { get; set; }

        /// <summary>
        ///     subexpression
        /// </summary>
        public SyntaxPartBase Subexpression { get; set; }

        /// <summary>
        ///     real number
        /// </summary>
        public SyntaxPartBase RealNumber { get; set; }

        /// <summary>
        ///     referenced label
        /// </summary>
        public SyntaxPartBase Label { get; internal set; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; set; }

        /// <summary>
        ///     open parenthesis
        /// </summary>
        public Terminal OpenParen { get; set; }

        /// <summary>
        ///     close parenthesis
        /// </summary>
        public Terminal CloseParen { get; set; }

        /// <summary>
        ///     open braces
        /// </summary>
        public Terminal OpenBraces { get; set; }

        /// <summary>
        ///     close braces
        /// </summary>
        public Terminal CloseBraces { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, SegmentPrefix, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, SegmentExpression, visitor);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, Subexpression, visitor);
            AcceptPart(this, CloseParen, visitor);
            AcceptPart(this, OpenBraces, visitor);
            AcceptPart(this, MemorySubexpression, visitor);
            AcceptPart(this, CloseBraces, visitor);
            AcceptPart(this, Identifier, visitor);
            AcceptPart(this, Number, visitor);
            AcceptPart(this, RealNumber, visitor);
            AcceptPart(this, HexNumber, visitor);
            AcceptPart(this, QuotedString, visitor);
            AcceptPart(this, Label, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => SegmentPrefix.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                SegmentExpression.GetSymbolLength() +
                OpenParen.GetSymbolLength() +
                Subexpression.GetSymbolLength() +
                CloseParen.GetSymbolLength() +
                OpenBraces.GetSymbolLength() +
                MemorySubexpression.GetSymbolLength() +
                CloseBraces.GetSymbolLength() +
                Identifier.GetSymbolLength() +
                Number.GetSymbolLength() +
                RealNumber.GetSymbolLength() +
                HexNumber.GetSymbolLength() +
                QuotedString.GetSymbolLength() +
                Label.GetSymbolLength();

    }
}
