using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     expression
    /// </summary>
    public class Expression : StandardSyntaxTreeBase {

        /// <summary>
        ///     closue expression
        /// </summary>
        public ClosureExpression ClosureExpression { get; set; }

        /// <summary>
        ///     relational operator kind
        /// </summary>
        public int Kind { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     simple expression
        /// </summary>
        public SimpleExpression LeftOperand { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public SimpleExpression RightOperand { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}
