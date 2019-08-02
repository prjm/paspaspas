using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     type section
    /// </summary>
    public class TypeSectionSymbol : VariableLengthSyntaxTreeBase<TypeDeclarationSymbol> {

        /// <summary>
        ///     create a new type section
        /// </summary>
        /// <param name="typeKeyword"></param>
        /// <param name="items"></param>
        public TypeSectionSymbol(Terminal typeKeyword, ImmutableArray<TypeDeclarationSymbol> items) : base(items)
            => TypeKeyword = typeKeyword;

        /// <summary>
        ///     type keyword
        /// </summary>
        public Terminal TypeKeyword { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, TypeKeyword, visitor);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => TypeKeyword.GetSymbolLength() + ItemLength;


    }
}