using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     case label
    /// </summary>
    public class CaseLabel : StandardSyntaxTreeBase {

        /// <summary>
        ///     end expression
        /// </summary>
        public Expression EndExpression { get; set; }

        /// <summary>
        ///     start expression
        /// </summary>
        public Expression StartExpression { get; set; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }

    }
}