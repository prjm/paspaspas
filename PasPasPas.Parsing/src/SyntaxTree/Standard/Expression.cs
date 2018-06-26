using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     expression
    /// </summary>
    public class Expression : StandardSyntaxTreeBase {

        /// <summary>
        ///     closure expression
        /// </summary>
        public ClosureExpressionSymbol ClosureExpression { get; set; }

        /// <summary>
        ///     relational operator kind
        /// </summary>
        public int Kind { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     simple expression
        /// </summary>
        public SimpleExpression LeftOperand { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public SimpleExpression RightOperand { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ClosureExpression.GetSymbolLength() +
               LeftOperand.GetSymbolLength() +
               RightOperand.GetSymbolLength();


    }
}
