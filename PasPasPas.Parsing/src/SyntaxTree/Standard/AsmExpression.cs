using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly expression term
    /// </summary>
    public class AsmExpression : StandardSyntaxTreeBase {

        /// <summary>
        ///     byte pointer
        /// </summary>
        public AsmOperand BytePtr { get; set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public AsmTerm LeftOperand { get; set; }

        /// <summary>
        ///     offset expression
        /// </summary>
        public AsmOperand Offset { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public AsmOperand RightOperand { get; set; }

        /// <summary>
        ///     type expression
        /// </summary>
        public AsmOperand TypeExpression { get; set; }

        /// <summary>
        ///     byte pointer kind
        /// </summary>
        public Identifier BytePtrKind { get; set; }

        /// <summary>
        ///     token kind
        /// </summary>
        public int BinaryOperatorKind { get; set; } = TokenKind.Undefined;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
