using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     term
    /// </summary>
    public class Term : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new term
        /// </summary>
        /// <param name="leftOperand"></param>
        /// <param name="operator"></param>
        /// <param name="rightOperand"></param>
        public Term(Factor leftOperand, Terminal @operator, Term rightOperand) {
            LeftOperand = leftOperand;
            Operator = @operator;
            RightOperand = rightOperand;
        }


        /// <summary>
        ///     operator
        /// </summary>
        public Terminal Operator { get; }

        /// <summary>
        ///     term kind
        /// </summary>
        public int Kind
            => Operator.GetSymbolKind();

        /// <summary>
        ///     left operand
        /// </summary>
        public Factor LeftOperand { get; }

        /// <summary>
        ///     right operand
        /// </summary>
        public Term RightOperand { get; }

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