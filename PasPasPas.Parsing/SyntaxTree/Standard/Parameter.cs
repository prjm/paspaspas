using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     parameter for a method call
    /// </summary>
    public class Parameter : StandardSyntaxTreeBase {

        /// <summary>
        ///     parameter expression
        /// </summary>
        public FormattedExpression Expression { get; internal set; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public Identifier ParameterName { get; internal set; }

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
