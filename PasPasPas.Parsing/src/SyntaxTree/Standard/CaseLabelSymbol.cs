using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     case label
    /// </summary>
    public class CaseLabelSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     end expression
        /// </summary>
        public SyntaxPartBase EndExpression { get; set; }

        /// <summary>
        ///     start expression
        /// </summary>
        public Expression StartExpression { get; set; }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; set; }

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal Dots { get; set; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, StartExpression, visitor);
            AcceptPart(this, Dots, visitor);
            AcceptPart(this, EndExpression, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => StartExpression.Length + Dots.Length + EndExpression.Length;

    }
}