using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly operand
    /// </summary>
    public class AsmOperand : StandardSyntaxTreeBase {

        /// <summary>
        ///     left term
        /// </summary>
        public AsmExpressionSymbol LeftTerm { get; internal set; }

        /// <summary>
        ///     not expression
        /// </summary>
        public AsmOperand NotExpression { get; set; }

        /// <summary>
        ///     right term
        /// </summary>
        public AsmOperand RightTerm { get; set; }

        /// <summary>
        ///     operand kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
