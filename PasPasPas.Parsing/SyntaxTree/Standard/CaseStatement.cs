using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     case statement
    /// </summary>
    public class CaseStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     case expression
        /// </summary>
        public Expression CaseExpression { get; set; }

        /// <summary>
        ///     else part
        /// </summary>
        public StatementList Else { get; set; }


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