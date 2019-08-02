using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     one part of a variant part of a record
    /// </summary>
    public class RecordVariantSymbol : VariableLengthSyntaxTreeBase<ConstantExpressionSymbol> {

        /// <summary>
        ///     create a new record variant symbol
        /// </summary>
        /// <param name="items"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="openParen"></param>
        /// <param name="fieldList"></param>
        /// <param name="variantSection"></param>
        /// <param name="closeParen"></param>
        /// <param name="semicolon"></param>
        public RecordVariantSymbol(ImmutableArray<ConstantExpressionSymbol> items, Terminal colonSymbol, Terminal openParen, RecordFieldListSymbol fieldList, RecordVariantSectionSymbol variantSection, Terminal closeParen, Terminal semicolon) : base(items) {
            ColonSymbol = colonSymbol;
            OpenParen = openParen;
            FieldList = fieldList;
            VariantSection = variantSection;
            CloseParen = closeParen;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     field list
        /// </summary>
        public RecordFieldListSymbol FieldList { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     open paren
        /// </summary>
        public Terminal OpenParen { get; }

        /// <summary>
        ///     variant section
        /// </summary>
        public RecordVariantSectionSymbol VariantSection { get; }

        /// <summary>
        ///     close paren
        /// </summary>
        public Terminal CloseParen { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, FieldList, visitor);
            AcceptPart(this, VariantSection, visitor);
            AcceptPart(this, CloseParen, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength +
                ColonSymbol.GetSymbolLength() +
                OpenParen.GetSymbolLength() +
                FieldList.GetSymbolLength() +
                VariantSection.GetSymbolLength() +
                CloseParen.GetSymbolLength() +
                Semicolon.GetSymbolLength();

    }
}