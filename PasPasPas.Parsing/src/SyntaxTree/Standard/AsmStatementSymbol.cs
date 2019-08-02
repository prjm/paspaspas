using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     asm statement
    /// </summary>
    public class AsmStatementSymbol : VariableLengthSyntaxTreeBase<AsmOperandSymbol> {

        /// <summary>
        ///     create a new asm statement symbol
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="prefix"></param>
        /// <param name="label"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="items"></param>
        public AsmStatementSymbol(
            AsmOpCodeSymbol opCode,
            AsmPrefixSymbol prefix,
            AsmLabelSymbol label,
            Terminal colonSymbol,
            ImmutableArray<AsmOperandSymbol> items) : base(items) {

            OpCode = opCode;
            Prefix = prefix;
            Label = label;
            ColonSymbol = colonSymbol;
        }

        /// <summary>
        ///     opcode
        /// </summary>
        public AsmOpCodeSymbol OpCode { get; }

        /// <summary>
        ///     lock / segment prefix
        /// </summary>
        public AsmPrefixSymbol Prefix { get; }

        /// <summary>
        ///     label
        /// </summary>
        public AsmLabelSymbol Label { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Label.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                Prefix.GetSymbolLength() +
                OpCode.GetSymbolLength() +
                ItemLength;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Label, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, Prefix, visitor);
            AcceptPart(this, OpCode, visitor);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
