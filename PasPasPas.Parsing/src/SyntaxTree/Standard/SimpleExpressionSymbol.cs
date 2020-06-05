#nullable disable
using PasPasPas.Globals.Parsing;

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
        public SimpleExpression(ISyntaxPart leftOperand, Terminal @operator, ISyntaxPart rightOperand) {
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
        public ISyntaxPart LeftOperand { get; }

        /// <summary>
        ///     right operand
        /// </summary>
        public ISyntaxPart RightOperand { get; }

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