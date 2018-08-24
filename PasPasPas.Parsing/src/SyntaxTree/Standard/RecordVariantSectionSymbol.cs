using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variant section
    /// </summary>
    public class RecordVariantSectionSymbol : VariableLengthSyntaxTreeBase<RecordVariantSymbol> {

        /// <summary>
        ///     create a new record variant section
        /// </summary>
        /// <param name="caseSymbol"></param>
        /// <param name="variantName"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="typeDeclaration"></param>
        /// <param name="ofSymbol"></param>
        /// <param name="items"></param>
        public RecordVariantSectionSymbol(Terminal caseSymbol, IdentifierSymbol variantName, Terminal colonSymbol, TypeSpecificationSymbol typeDeclaration, Terminal ofSymbol, ImmutableArray<RecordVariantSymbol> items) : base(items) {
            CaseSymbol = caseSymbol;
            Name = variantName;
            ColonSymbol = colonSymbol;
            TypeDeclaration = typeDeclaration;
            OfSymbol = ofSymbol;
        }

        /// <summary>
        ///     name of the variant
        /// </summary>
        public IdentifierSymbol Name { get; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecificationSymbol TypeDeclaration { get; }

        /// <summary>
        ///     case symbol
        /// </summary>
        public Terminal CaseSymbol { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     of symbol
        /// </summary>
        public Terminal OfSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, CaseSymbol, visitor);
            AcceptPart(this, Name, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, TypeDeclaration, visitor);
            AcceptPart(this, OfSymbol, visitor);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => CaseSymbol.GetSymbolLength() +
                Name.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                TypeDeclaration.GetSymbolLength() +
                OfSymbol.GetSymbolLength() +
                ItemLength;

    }
}