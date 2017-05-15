using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     term
    /// </summary>
    public class Term : StandardSyntaxTreeBase {

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
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}