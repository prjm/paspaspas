using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     simple expression
    /// </summary>
    public class SimpleExpression : StandardSyntaxTreeBase {
        public SimpleExpression(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

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
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }


    }
}