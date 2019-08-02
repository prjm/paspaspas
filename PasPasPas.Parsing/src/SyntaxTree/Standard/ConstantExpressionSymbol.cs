using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     constant expression
    /// </summary>
    public class ConstantExpressionSymbol : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     create a new constant expression symbol
        /// </summary>
        /// <param name="closeParen"></param>
        /// <param name="items"></param>
        /// <param name="openParen"></param>
        /// <param name="comma"></param>
        public ConstantExpressionSymbol(Terminal openParen, ImmutableArray<SyntaxPartBase> items, Terminal closeParen, Terminal comma) : base(items) {
            IsRecordConstant = true;
            OpenParen = openParen;
            CloseParen = closeParen;
            Comma = comma;
        }

        /// <summary>
        ///     create a new constant expression symbol
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="comma"></param>
        public ConstantExpressionSymbol(ExpressionSymbol expression, Terminal comma) : base(ImmutableArray<SyntaxPartBase>.Empty) {
            Value = expression;
            Comma = comma;
        }

        /// <summary>
        ///     create a new constant expression
        /// </summary>
        /// <param name="openParen"></param>
        /// <param name="closeParen"></param>
        /// <param name="items"></param>
        /// <param name="comma"></param>
        public ConstantExpressionSymbol(Terminal openParen, Terminal closeParen, ImmutableArray<SyntaxPartBase> items, Terminal comma) : base(items) {
            IsArrayConstant = true;
            OpenParen = openParen;
            CloseParen = closeParen;
            Comma = comma;
        }

        /// <summary>
        ///     <c>true</c> if this is an array constant
        /// </summary>
        public bool IsArrayConstant { get; }

        /// <summary>
        ///     <c>true</c> if this in an record constant
        /// </summary>
        public bool IsRecordConstant { get; }

        /// <summary>
        ///     value of the expression
        /// </summary>
        public ExpressionSymbol Value { get; }

        /// <summary>
        ///     open parenthesis
        /// </summary>
        public Terminal OpenParen { get; }

        /// <summary>
        ///     close parenthesis
        /// </summary>
        public Terminal CloseParen { get; }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, CloseParen, visitor);
            AcceptPart(this, Value, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     token length
        /// </summary>
        public override int Length
            => OpenParen.GetSymbolLength() +
               ItemLength +
               CloseParen.GetSymbolLength() +
               Value.GetSymbolLength() +
               Comma.GetSymbolLength();


    }
}