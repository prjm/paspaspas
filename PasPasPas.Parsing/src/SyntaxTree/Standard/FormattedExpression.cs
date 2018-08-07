using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formatted expression
    /// </summary>
    public class FormattedExpression : StandardSyntaxTreeBase {

        /// <summary>
        ///     decimals subexpression
        /// </summary>
        public ExpressionSymbol Decimals { get; set; }

        /// <summary>
        ///     width subexpression
        /// </summary>
        public ExpressionSymbol Width { get; set; }

        /// <summary>
        ///     base expression
        /// </summary>
        public ExpressionSymbol Expression { get; set; }

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