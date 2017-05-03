using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly operand
    /// </summary>
    public class AsmOperand : StandardSyntaxTreeBase {

        /// <summary>
        ///     left term
        /// </summary>
        public AsmExpression LeftTerm { get; internal set; }

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
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }

    }
}
