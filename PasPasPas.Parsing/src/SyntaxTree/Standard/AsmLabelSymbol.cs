using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     label in <c>asm</c>
    /// </summary>
    public class AsmLabelSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     asm label
        /// </summary>
        public SyntaxPartBase LocalLabel { get; set; }

        /// <summary>
        ///     generic label
        /// </summary>
        public SyntaxPartBase Label { get; set; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => LocalLabel.Length + Label.Length;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, LocalLabel, visitor);
            AcceptPart(this, Label, visitor);
            visitor.EndVisit(this);
        }
    }
}
