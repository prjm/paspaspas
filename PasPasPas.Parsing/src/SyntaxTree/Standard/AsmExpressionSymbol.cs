using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly expression term
    /// </summary>
    public class AsmExpressionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     byte pointer
        /// </summary>
        public SyntaxPartBase BytePtr { get; set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public SyntaxPartBase LeftOperand { get; set; }

        /// <summary>
        ///     offset expression
        /// </summary>
        public SyntaxPartBase Offset { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public SyntaxPartBase RightOperand { get; set; }

        /// <summary>
        ///     type expression
        /// </summary>
        public SyntaxPartBase TypeExpression { get; set; }

        /// <summary>
        ///     byte pointer kind
        /// </summary>
        public SyntaxPartBase BytePtrKind { get; set; }

        /// <summary>
        ///     token kind
        /// </summary>
        public int BinaryOperatorKind { get; set; } = TokenKind.Undefined;

        /// <summary>
        ///     offset symbol
        /// </summary>
        public Terminal OffsetSymbol { get; set; }

        /// <summary>
        ///     type symbol
        /// </summary>
        public Terminal TypeSymbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OffsetSymbol, visitor);
            AcceptPart(this, Offset, visitor);
            AcceptPart(this, BytePtrKind, visitor);
            AcceptPart(this, BytePtr, visitor);
            AcceptPart(this, TypeSymbol, visitor);
            AcceptPart(this, TypeExpression, visitor);
            AcceptPart(this, LeftOperand, visitor);
            AcceptPart(this, RightOperand, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => Offset.Length +
                OffsetSymbol.Length +
                BytePtr.Length +
                BytePtrKind.Length +
                TypeSymbol.Length +
                TypeExpression.Length +
                LeftOperand.Length +
                RightOperand.Length;


    }
}

