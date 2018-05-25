using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly operand
    /// </summary>
    public class AsmOperandSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     left term
        /// </summary>
        public SyntaxPartBase LeftTerm { get; set; }

        /// <summary>
        ///     not expression
        /// </summary>
        public SyntaxPartBase NotExpression { get; set; }

        /// <summary>
        ///     right term
        /// </summary>
        public SyntaxPartBase RightTerm { get; set; }

        /// <summary>
        ///     operand kind
        /// </summary>
        public int Kind
            => Operand.Kind;

        /// <summary>
        ///     not symbol
        /// </summary>
        public Terminal NotSymbol { get; set; }

        /// <summary>
        ///     operand
        /// </summary>
        public Terminal Operand { get; set; }

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
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => NotSymbol.Length + NotExpression.Length + LeftTerm.Length + Operand.Length + RightTerm.Length + Comma.Length;

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; set; }
    }
}
