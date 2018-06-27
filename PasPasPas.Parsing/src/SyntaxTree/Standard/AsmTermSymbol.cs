using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly term
    /// </summary>
    public class AsmTermSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     left operand
        /// </summary>
        public AsmFactorSymbol LeftOperand { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public SyntaxPartBase RightOperand { get; set; }

        /// <summary>
        ///     subtype
        /// </summary>
        public SyntaxPartBase Subtype { get; set; }

        /// <summary>
        ///     term kind
        /// </summary>
        public int Kind
            => Operator.Kind;

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal DotSymbol { get; set; }

        /// <summary>
        ///     operator
        /// </summary>
        public Terminal Operator { get; set; }

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
