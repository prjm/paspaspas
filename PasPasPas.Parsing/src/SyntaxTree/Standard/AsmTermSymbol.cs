using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly term
    /// </summary>
    public class AsmTermSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new term symbol
        /// </summary>
        /// <param name="leftOperand"></param>
        /// <param name="dotSymbol"></param>
        /// <param name="subtype"></param>
        /// <param name="oprator"></param>
        /// <param name="rightOperand"></param>
        public AsmTermSymbol(AsmFactorSymbol leftOperand, Terminal dotSymbol, AsmOperandSymbol subtype, Terminal oprator, AsmOperandSymbol rightOperand) {
            LeftOperand = leftOperand;
            DotSymbol = dotSymbol;
            Subtype = subtype;
            RightOperand = rightOperand;
            Operator = oprator;
        }

        /// <summary>
        ///     left operand
        /// </summary>
        public AsmFactorSymbol LeftOperand { get; }

        /// <summary>
        ///     right operand
        /// </summary>
        public AsmOperandSymbol RightOperand { get; }

        /// <summary>
        ///     subtype
        /// </summary>
        public AsmOperandSymbol Subtype { get; }

        /// <summary>
        ///     term kind
        /// </summary>
        public int Kind
            => Operator.Kind;

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal DotSymbol { get; }

        /// <summary>
        ///     operator
        /// </summary>
        public Terminal Operator { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, DotSymbol, visitor);
            AcceptPart(this, Subtype, visitor);
            AcceptPart(this, LeftOperand, visitor);
            AcceptPart(this, Operator, visitor);
            AcceptPart(this, RightOperand, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => DotSymbol.GetSymbolLength() +
                Subtype.GetSymbolLength() +
                LeftOperand.GetSymbolLength() +
                Operator.GetSymbolLength() +
                RightOperand.GetSymbolLength();

    }
}
