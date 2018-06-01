using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     external directive
    /// </summary>
    public class ExternalDirective : StandardSyntaxTreeBase {


        /// <summary>
        ///     external expression
        /// </summary>
        public ConstantExpressionSymbol ExternalExpression { get; set; }

        /// <summary>
        ///     kind
        /// </summary>
        public int Kind { get; set; }

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
