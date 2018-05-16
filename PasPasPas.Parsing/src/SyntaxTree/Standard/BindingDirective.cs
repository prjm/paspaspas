using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     binding directive
    /// </summary>
    public class BindingDirective : StandardSyntaxTreeBase {

        /// <summary>
        ///     directive kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     message expression
        /// </summary>
        public Expression MessageExpression { get; internal set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
