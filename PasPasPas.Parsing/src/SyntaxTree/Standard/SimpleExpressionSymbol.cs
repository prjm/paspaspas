using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     simple expression
    /// </summary>
    public class SimpleExpression : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new simple expression
        /// </summary>
        /// <param name="leftOperand"></param>
        /// <param name="operator"></param>
        /// <param name="rightOperand"></param>
        public SimpleExpression(TermSymbol leftOperand, Terminal @operator, SimpleExpression rightOperand) {
            LeftOperand = leftOperand;
            Operator = @operator;
            RightOperand = rightOperand;
        }

        /// <summary>
        ///     operator
        /// </summary>
        public Terminal Operator { get; }

        /// <summary>
        ///     expression kind
        /// </summary>
        public int Kind
            => Operator.GetSymbolKind();

        /// <summary>
        ///     left operand
        /// </summary>
        public TermSymbol LeftOperand { get; }

        /// <summary>
        ///     right operand
        /// </summary>
        public SimpleExpression RightOperand { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, LeftOperand, visitor);
            AcceptPart(this, Operator, visitor);
            AcceptPart(this, RightOperand, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => LeftOperand.GetSymbolLength() +
                Operator.GetSymbolLength() +
                RightOperand.GetSymbolLength();


    }
}