using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     simple expression
    /// </summary>
    public class SimpleExpression : StandardSyntaxTreeBase {

        /// <summary>
        ///     expression kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public Term LeftOperand { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public SimpleExpression RightOperand { get; set; }

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