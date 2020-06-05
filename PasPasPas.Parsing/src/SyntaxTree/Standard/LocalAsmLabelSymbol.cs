#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     local asm label
    /// </summary>
    public class LocalAsmLabelSymbol : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     at symbol
        /// </summary>
        public Terminal AtSymbol { get; }

        /// <summary>
        ///     create a new local symbol
        /// </summary>
        /// <param name="at"></param>
        /// <param name="labels"></param>
        public LocalAsmLabelSymbol(Terminal at, ImmutableArray<SyntaxPartBase> labels) : base(labels)
            => AtSymbol = at;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, AtSymbol, visitor);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length =>
            AtSymbol.GetSymbolLength() + ItemLength;

    }
}

