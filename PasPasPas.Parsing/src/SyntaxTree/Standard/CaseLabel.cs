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
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}