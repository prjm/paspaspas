using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formatted expression
    /// </summary>
    public class FormattedExpression : StandardSyntaxTreeBase {
        public FormattedExpression(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     decimals subexpression
        /// </summary>
        public Expression Decimals { get; set; }

        /// <summary>
        ///     width subexpression
        /// </summary>
        public Expression Width { get; set; }

        /// <summary>
        ///     base expression
        /// </summary>
        public Expression Expression { get; set; }

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