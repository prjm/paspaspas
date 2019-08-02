using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     var section
    /// </summary>
    public class VarSection : VariableLengthSyntaxTreeBase<VarDeclaration> {

        /// <summary>
        ///     create a new var section
        /// </summary>
        /// <param name="varSymbol"></param>
        /// <param name="items"></param>
        public VarSection(Terminal varSymbol, ImmutableArray<VarDeclaration> items) : base(items)
            => VarSymbol = varSymbol;

        /// <summary>
        ///     section kind: var or threadvar
        /// </summary>
        public int Kind
            => VarSymbol.GetSymbolKind();

        /// <summary>
        ///     var symbol
        /// </summary>
        public Terminal VarSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, VarSymbol, visitor);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => VarSymbol.GetSymbolLength() + ItemLength;

    }
}
