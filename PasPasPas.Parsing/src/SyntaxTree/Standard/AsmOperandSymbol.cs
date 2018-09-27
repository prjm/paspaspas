using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly operand
    /// </summary>
    public class AsmOperandSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new operand symbol
        /// </summary>
        /// <param name="notSymbol"></param>
        /// <param name="notExpression"></param>
        /// <param name="comma">comma</param>
        public AsmOperandSymbol(Terminal notSymbol, AsmOperandSymbol notExpression, Terminal comma) {
            NotSymbol = notSymbol;
            NotExpression = notExpression;
            Comma = comma;
        }

        /// <summary>
        ///     create a new operand symbol
        /// </summary>
        /// <param name="leftTerm"></param>
        /// <param name="operand"></param>
        /// <param name="rightTerm"></param>
        /// <param name="comma"></param>
        public AsmOperandSymbol(AsmExpressionSymbol leftTerm, Terminal operand, AsmOperandSymbol rightTerm, Terminal comma) {
            LeftTerm = leftTerm;
            Operand = operand;
            RightTerm = rightTerm;
            Comma = comma;
        }

        /// <summary>
        ///     left term
        /// </summary>
        public SyntaxPartBase LeftTerm { get; }

        /// <summary>
        ///     not expression
        /// </summary>
        public SyntaxPartBase NotExpression { get; }

        /// <summary>
        ///     right term
        /// </summary>
        public SyntaxPartBase RightTerm { get; }

        /// <summary>
        ///     operand kind
        /// </summary>
        public int Kind
            => Operand.GetSymbolKind();

        /// <summary>
        ///     not symbol
        /// </summary>
        public Terminal NotSymbol { get; }

        /// <summary>
        ///     operand
        /// </summary>
        public Terminal Operand { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, NotSymbol, visitor);
            AcceptPart(this, NotExpression, visitor);
            AcceptPart(this, LeftTerm, visitor);
            AcceptPart(this, Operand, visitor);
            AcceptPart(this, RightTerm, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => NotSymbol.GetSymbolLength() +
                NotExpression.GetSymbolLength() +
                LeftTerm.GetSymbolLength() +
                Operand.GetSymbolLength() +
                RightTerm.GetSymbolLength() +
                Comma.GetSymbolLength();

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }
    }
}
