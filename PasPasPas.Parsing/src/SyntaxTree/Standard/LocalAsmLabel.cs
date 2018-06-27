using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     local asm label
    /// </summary>
    public class LocalAsmLabel : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     at symbol
        /// </summary>
        private Terminal AtSymbol { get; }

        /// <summary>
        ///     create a new local symbol
        /// </summary>
        /// <param name="at"></param>
        /// <param name="labels"></param>
        public LocalAsmLabel(Terminal at, ImmutableArray<SyntaxPartBase> labels) : base(labels) {
            AtSymbol = at;
        }

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

