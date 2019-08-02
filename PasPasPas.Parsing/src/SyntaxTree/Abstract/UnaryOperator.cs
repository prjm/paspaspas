using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     unary operator
    /// </summary>
    public class UnaryOperator : ExpressionBase, IExpressionTarget {

        /// <summary>
        ///     operator kind
        /// </summary>
        public ExpressionKind Kind { get; set; }

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Value, visitor);
            visitor.EndVisit(this);
        }

    }
}
