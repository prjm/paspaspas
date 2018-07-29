using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     factor
    /// </summary>
    public class Factor : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new factory with an unary operator
        /// </summary>
        /// <param name="unaryOperator"></param>
        /// <param name="factor"></param>
        public Factor(Terminal unaryOperator, Factor factor) {
            UnaryOperator = unaryOperator;
            UnaryOperand = factor;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="unaryOperator"></param>
        /// <param name="identifier"></param>
        public Factor(Terminal unaryOperator, Identifier identifier) {
            UnaryOperator = unaryOperator;
            PointerTo = identifier;
        }

        /// <summary>
        ///  create a new factor
        /// </summary>
        /// <param name="intValue"></param>
        /// <param name="designator"></param>
        public Factor(StandardInteger intValue, DesignatorStatementSymbol designator) {
            IntValue = intValue;
            Designator = designator;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="hexValue"></param>
        /// <param name="designator"></param>
        public Factor(HexNumber hexValue, DesignatorStatementSymbol designator) {
            HexValue = hexValue;
            Designator = designator;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="realValue"></param>
        /// <param name="designator"></param>
        public Factor(RealNumberSymbol realValue, DesignatorStatementSymbol designator) {
            RealValue = realValue;
            Designator = designator;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="designator"></param>
        public Factor(QuotedString stringValue, DesignatorStatementSymbol designator) {
            StringValue = stringValue;
            Designator = designator;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="booleanValue"></param>
        /// <param name="designator"></param>
        public Factor(Terminal booleanValue, DesignatorStatementSymbol designator) {
            UnaryOperator = booleanValue;
            Designator = designator;
        }

        /// <summary>
        ///     create a new factory
        /// </summary>
        /// <param name="nilSymbol"></param>
        public Factor(Terminal nilSymbol)
            => UnaryOperator = nilSymbol;

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="designatorStatement"></param>
        public Factor(DesignatorStatementSymbol designatorStatement)
            => Designator = designatorStatement;

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="openParen"></param>
        /// <param name="parenExpression"></param>
        /// <param name="closeParen"></param>
        public Factor(Terminal openParen, ConstantExpressionSymbol parenExpression, Terminal closeParen) : this(openParen) {
            OpenParen = openParen;
            ParenExpression = parenExpression;
            CloseParen = closeParen;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="setSection"></param>
        public Factor(SetSection setSection)
            => SetSection = setSection;

        /// <summary>
        ///     open paren
        /// </summary>
        public Terminal OpenParen { get; }

        /// <summary>
        ///     close paren
        /// </summary>
        public Terminal CloseParen { get; }

        /// <summary>
        ///     unary operator
        /// </summary>
        public Terminal UnaryOperator { get; }

        /// <summary>
        ///     address of operator
        /// </summary>
        public Factor UnaryOperand { get; }

        /// <summary>
        ///     designator (inherited)
        /// </summary>
        public DesignatorStatementSymbol Designator { get; }

        /// <summary>
        ///     hex number
        /// </summary>
        public HexNumber HexValue { get; }

        /// <summary>
        ///     int value
        /// </summary>
        public StandardInteger IntValue { get; }

        /// <summary>
        ///     <c>false</c> literal
        /// </summary>
        public bool IsFalse
            => UnaryOperator.GetSymbolKind() == TokenKind.False;

        /// <summary>
        ///     <c>nil</c> literal
        /// </summary>
        public bool IsNil
            => UnaryOperator.GetSymbolKind() == TokenKind.Nil;

        /// <summary>
        ///     <c>true</c> literal
        /// </summary>
        public bool IsTrue
            => UnaryOperator.GetSymbolKind() == TokenKind.True;

        /// <summary>
        ///     parented expression
        /// </summary>
        public ConstantExpressionSymbol ParenExpression { get; }

        /// <summary>
        ///     pointer to
        /// </summary>
        public Identifier PointerTo { get; }

        /// <summary>
        ///     real value
        /// </summary>
        public RealNumberSymbol RealValue { get; }

        /// <summary>
        ///     set section
        /// </summary>
        public SetSection SetSection { get; }

        /// <summary>
        ///     string factor
        /// </summary>
        public QuotedString StringValue { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, UnaryOperator, visitor);
            AcceptPart(this, UnaryOperand, visitor);
            AcceptPart(this, SetSection, visitor);
            AcceptPart(this, IntValue, visitor);
            AcceptPart(this, HexValue, visitor);
            AcceptPart(this, StringValue, visitor);
            AcceptPart(this, RealValue, visitor);
            AcceptPart(this, PointerTo, visitor);
            AcceptPart(this, Designator, visitor);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, ParenExpression, visitor);
            AcceptPart(this, CloseParen, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => UnaryOperator.GetSymbolLength() +
                UnaryOperand.GetSymbolLength() +
                IntValue.GetSymbolLength() +
                StringValue.GetSymbolLength() +
                RealValue.GetSymbolLength() +
                HexValue.GetSymbolLength() +
                PointerTo.GetSymbolLength() +
                Designator.GetSymbolLength() +
                OpenParen.GetSymbolLength() +
                ParenExpression.GetSymbolLength() +
                SetSection.GetSymbolLength() +
                CloseParen.GetSymbolLength();

    }
}