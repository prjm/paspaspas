using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     array index definition
    /// </summary>
    public class ArrayIndex : StandardSyntaxTreeBase {

        /// <summary>
        ///     start index
        /// </summary>
        public ConstantExpression StartIndex { get; set; }

        /// <summary>
        ///     end index
        /// </summary>
        public ConstantExpression EndIndex { get; set; }

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