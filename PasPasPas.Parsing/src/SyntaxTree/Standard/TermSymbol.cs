using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     term
    /// </summary>
    public class TermSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new term
        /// </summary>
        /// <param name="leftOperand"></param>
        /// <param name="operator"></param>
        /// <param name="rightOperand"></param>
        public TermSymbol(FactorSymbol leftOperand, Terminal @operator, ISyntaxPart rightOperand) {
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
        public FactorSymbol LeftOperand { get; }

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