using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     section for constants
    /// </summary>
    public class ConstSectionSymbol : VariableLengthSyntaxTreeBase<ConstDeclarationSymbol> {

        /// <summary>
        ///     create a new const section
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="items"></param>
        public ConstSectionSymbol(Terminal symbol, ImmutableArray<ConstDeclarationSymbol> items) : base(items) => ConstSymbol = symbol;

        /// <summary>
        ///     constant symbol
        /// </summary>
        public Terminal ConstSymbol { get; }

        /// <summary>
        ///     kind of constant
        /// </summary>
        public int Kind
                => ConstSymbol.GetSymbolKind();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ConstSymbol, visitor);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     item length
        /// </summary>
        public override int Length =>
            ConstSymbol.GetSymbolLength() + ItemLength;

    }
}
