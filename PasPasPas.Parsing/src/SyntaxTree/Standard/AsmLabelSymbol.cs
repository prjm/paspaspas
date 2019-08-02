using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     label in <c>asm</c>
    /// </summary>
    public class AsmLabelSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     local asm label
        /// </summary>
        /// <param name="localAsmLabel"></param>
        public AsmLabelSymbol(LocalAsmLabelSymbol localAsmLabel)
            => LocalLabel = localAsmLabel;

        /// <summary>
        ///     standard label
        /// </summary>
        /// <param name="label"></param>
        public AsmLabelSymbol(LabelSymbol label)
            => Label = label;

        /// <summary>
        ///     asm label
        /// </summary>
        public LocalAsmLabelSymbol LocalLabel { get; }

        /// <summary>
        ///     generic label
        /// </summary>
        public LabelSymbol Label { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => LocalLabel.GetSymbolLength() + Label.GetSymbolLength();

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
