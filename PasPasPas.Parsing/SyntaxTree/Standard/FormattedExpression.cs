using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formatted expression
    /// </summary>
    public class FormattedExpression : StandardSyntaxTreeBase {

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
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }



    }
}