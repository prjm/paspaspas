using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly term   
    /// </summary>
    public class AsmTerm : StandardSyntaxTreeBase {

        /// <summary>
        ///     left operand
        /// </summary>
        public AsmFactor LeftOperand { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public AsmOperand RightOperand { get; set; }

        /// <summary>
        ///     subtype
        /// </summary>
        public AsmOperand Subtype { get; set; }

        /// <summary>
        ///     term kind
        /// </summary>
        public int Kind { get; set; }
            = TokenKind.Undefined;

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
