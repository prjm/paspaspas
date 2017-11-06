using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     label in <c>asm</c>
    /// </summary>
    public class AsmLabel : StandardSyntaxTreeBase {

        /// <summary>
        ///     asm label
        /// </summary>
        public LocalAsmLabel LocalLabel { get; set; }

        /// <summary>
        ///     generic label
        /// </summary>
        public Label Label { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
