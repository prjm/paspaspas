using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     asm statement
    /// </summary>
    public class AsmStatementSymbolx : VariableLengthSyntaxTreeBase<AsmOperandSymbol> {

        public AsmStatementSymbolx(ImmutableArray<AsmOperandSymbol> items) : base(items) {

        }

        /// <summary>
        ///     opcode
        /// </summary>
        public SyntaxPartBase OpCode { get; set; }

        /// <summary>
        ///     lock / segment prefix
        /// </summary>
        public SyntaxPartBase Prefix { get; set; }

        /// <summary>
        ///     label
        /// </summary>
        public SyntaxPartBase Label { get; set; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; set; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => Label.Length + ColonSymbol.Length + Prefix.Length + OpCode.Length + ItemLength;

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
