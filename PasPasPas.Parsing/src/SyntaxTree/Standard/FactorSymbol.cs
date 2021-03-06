﻿#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     factor
    /// </summary>
    public class FactorSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new factory with an unary operator
        /// </summary>
        /// <param name="unaryOperator"></param>
        /// <param name="factor"></param>
        public FactorSymbol(Terminal unaryOperator, FactorSymbol factor) {
            UnaryOperator = unaryOperator;
            UnaryOperand = factor;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="unaryOperator"></param>
        /// <param name="identifier"></param>
        public FactorSymbol(Terminal unaryOperator, IdentifierSymbol identifier) {
            UnaryOperator = unaryOperator;
            PointerTo = identifier;
        }

        /// <summary>
        ///  create a new factor
        /// </summary>
        /// <param name="intValue"></param>
        /// <param name="designator"></param>
        public FactorSymbol(StandardInteger intValue, DesignatorStatementSymbol designator) {
            IntValue = intValue;
            Designator = designator;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="hexValue"></param>
        /// <param name="designator"></param>
        public FactorSymbol(HexNumberSymbol hexValue, DesignatorStatementSymbol designator) {
            HexValue = hexValue;
            Designator = designator;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="realValue"></param>
        /// <param name="designator"></param>
        public FactorSymbol(RealNumberSymbol realValue, DesignatorStatementSymbol designator) {
            RealValue = realValue;
            Designator = designator;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="designator"></param>
        public FactorSymbol(QuotedStringSymbol stringValue, DesignatorStatementSymbol designator) {
            StringValue = stringValue;
            Designator = designator;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="booleanValue"></param>
        /// <param name="designator"></param>
        public FactorSymbol(Terminal booleanValue, DesignatorStatementSymbol designator) {
            UnaryOperator = booleanValue;
            Designator = designator;
        }

        /// <summary>
        ///     create a new factory
        /// </summary>
        /// <param name="nilSymbol"></param>
        public FactorSymbol(Terminal nilSymbol)
            => UnaryOperator = nilSymbol;

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="designatorStatement"></param>
        public FactorSymbol(DesignatorStatementSymbol designatorStatement)
            => Designator = designatorStatement;

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="openParen"></param>
        /// <param name="parenExpression"></param>
        /// <param name="closeParen"></param>
        public FactorSymbol(Terminal openParen, ConstantExpressionSymbol parenExpression, Terminal closeParen) {
            OpenParen = openParen;
            ParenExpression = parenExpression;
            CloseParen = closeParen;
        }

        /// <summary>
        ///     create a new factor
        /// </summary>
        /// <param name="setSection"></param>
        public FactorSymbol(SetSectionSymbol setSection)
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
        public FactorSymbol UnaryOperand { get; }

        /// <summary>
        ///     designator (inherited)
        /// </summary>
        public DesignatorStatementSymbol Designator { get; }

        /// <summary>
        ///     hex number
        /// </summary>
        public HexNumberSymbol HexValue { get; }

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
        public IdentifierSymbol PointerTo { get; }

        /// <summary>
        ///     real value
        /// </summary>
        public RealNumberSymbol RealValue { get; }

        /// <summary>
        ///     set section
        /// </summary>
        public SetSectionSymbol SetSection { get; }

        /// <summary>
        ///     string factor
        /// </summary>
        public QuotedStringSymbol StringValue { get; }

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