using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     external specifier
    /// </summary>
    public class ExternalSpecifierSymbol : VariableLengthSyntaxTreeBase<ConstantExpressionSymbol> {

        /// <summary>
        ///     create a new external specifier
        /// </summary>
        /// <param name="specifier"></param>
        public ExternalSpecifierSymbol(Terminal specifier) : base(ImmutableArray<ConstantExpressionSymbol>.Empty)
            => Specifier = specifier;

        /// <summary>
        ///     create a new external specifier
        /// </summary>
        /// <param name="specifier"></param>
        /// <param name="items"></param>
        public ExternalSpecifierSymbol(Terminal specifier, ImmutableArray<ConstantExpressionSymbol> items) : base(items)
            => Specifier = specifier;

        /// <summary>
        ///     external specifier kind
        /// </summary>
        public int Kind
            => Specifier.GetSymbolKind();

        /// <summary>
        ///     specifier
        /// </summary>
        public Terminal Specifier { get; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Specifier, visitor);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length =>
            Specifier.GetSymbolLength() + ItemLength;

    }
}
