using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     term
    /// </summary>
    public class Term : StandardSyntaxTreeBase {
        public Term(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     term kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public Factor LeftOperand { get; set; }

        /// <summary>
        ///     rihgt operand
        /// </summary>
        public Term RightOperand { get; set; }

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