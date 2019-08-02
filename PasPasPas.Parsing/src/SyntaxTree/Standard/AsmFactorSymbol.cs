using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly factor
    /// </summary>
    public class AsmFactorSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create new asm factor
        /// </summary>
        /// <param name="openParen"></param>
        /// <param name="subexpression"></param>
        /// <param name="closeParen"></param>
        public AsmFactorSymbol(Terminal openParen, AsmOperandSymbol subexpression, Terminal closeParen) {
            OpenParen = openParen;
            Subexpression = subexpression;
            CloseParen = closeParen;
        }

        /// <summary>
        ///     create a new asm factor symbol
        /// </summary>
        /// <param name="segmentPrefix"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="segmentExpression"></param>
        public AsmFactorSymbol(IdentifierSymbol segmentPrefix, Terminal colonSymbol, AsmOperandSymbol segmentExpression) {
            SegmentPrefix = segmentPrefix;
            ColonSymbol = colonSymbol;
            SegmentExpression = segmentExpression;
        }

        /// <summary>
        ///     asm label
        /// </summary>
        /// <param name="localAsmLabel"></param>
        public AsmFactorSymbol(LocalAsmLabelSymbol localAsmLabel)
            => Label = localAsmLabel;

        /// <summary>
        ///     asm quoted string
        /// </summary>
        /// <param name="quotedString"></param>
        public AsmFactorSymbol(QuotedStringSymbol quotedString)
            => QuotedString = quotedString;

        /// <summary>
        ///     asm hex number
        /// </summary>
        /// <param name="hexNumber"></param>
        public AsmFactorSymbol(HexNumberSymbol hexNumber)
            => HexNumber = hexNumber;

        /// <summary>
        ///     asm real number
        /// </summary>
        /// <param name="realNumber"></param>
        public AsmFactorSymbol(RealNumberSymbol realNumber)
            => RealNumber = realNumber;

        /// <summary>
        ///     asm integer
        /// </summary>
        /// <param name="standardInteger"></param>
        public AsmFactorSymbol(StandardInteger standardInteger) =>
            Number = standardInteger;

        /// <summary>
        ///     asm identifier
        /// </summary>
        /// <param name="identifier"></param>
        public AsmFactorSymbol(IdentifierSymbol identifier)
            => Identifier = identifier;

        /// <summary>
        ///     asm subexpression
        /// </summary>
        /// <param name="openBraces"></param>
        /// <param name="closeBraces"></param>
        /// <param name="memorySubexpression"></param>
        public AsmFactorSymbol(Terminal openBraces, Terminal closeBraces, AsmOperandSymbol memorySubexpression) {
            OpenBraces = openBraces;
            CloseBraces = closeBraces;
            MemorySubexpression = memorySubexpression;
        }

        /// <summary>
        ///     hex number
        /// </summary>
        public HexNumberSymbol HexNumber { get; }

        /// <summary>
        ///     identifier
        /// </summary>
        public IdentifierSymbol Identifier { get; }

        /// <summary>
        ///     memory subexpression
        /// </summary>
        public AsmOperandSymbol MemorySubexpression { get; }

        /// <summary>
        ///     number
        /// </summary>
        public StandardInteger Number { get; }

        /// <summary>
        ///     quoted string
        /// </summary>
        public QuotedStringSymbol QuotedString { get; }

        /// <summary>
        ///     segment subexpression
        /// </summary>
        public AsmOperandSymbol SegmentExpression { get; }

        /// <summary>
        ///     segment prefix
        /// </summary>
        public IdentifierSymbol SegmentPrefix { get; }

        /// <summary>
        ///     subexpression
        /// </summary>
        public AsmOperandSymbol Subexpression { get; }

        /// <summary>
        ///     real number
        /// </summary>
        public RealNumberSymbol RealNumber { get; }

        /// <summary>
        ///     referenced label
        /// </summary>
        public LocalAsmLabelSymbol Label { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     open parenthesis
        /// </summary>
        public Terminal OpenParen { get; }

        /// <summary>
        ///     close parenthesis
        /// </summary>
        public Terminal CloseParen { get; }

        /// <summary>
        ///     open braces
        /// </summary>
        public Terminal OpenBraces { get; }

        /// <summary>
        ///     close braces
        /// </summary>
        public Terminal CloseBraces { get; }

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
