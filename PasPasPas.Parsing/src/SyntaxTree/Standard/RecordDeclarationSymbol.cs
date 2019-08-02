using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record declaration
    /// </summary>
    public class RecordDeclarationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create  new record declaration
        /// </summary>
        /// <param name="recordSymbol"></param>
        /// <param name="fieldList"></param>
        /// <param name="variantSection"></param>
        /// <param name="items"></param>
        /// <param name="endSymbol"></param>
        public RecordDeclarationSymbol(Terminal recordSymbol, RecordFieldListSymbol fieldList, RecordVariantSectionSymbol variantSection, RecordItemsSymbol items, Terminal endSymbol) {
            RecordSymbol = recordSymbol;
            FieldList = fieldList;
            VariantSection = variantSection;
            Items = items;
            EndSymbol = endSymbol;
        }

        /// <summary>
        ///     field list
        /// </summary>
        public RecordFieldListSymbol FieldList { get; }

        /// <summary>
        ///     record items
        /// </summary>
        public RecordItemsSymbol Items { get; }

        /// <summary>
        ///     variant section
        /// </summary>
        public RecordVariantSectionSymbol VariantSection { get; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }

        /// <summary>
        ///     record symbol
        /// </summary>
        public Terminal RecordSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, RecordSymbol, visitor);
            AcceptPart(this, FieldList, visitor);
            AcceptPart(this, VariantSection, visitor);
            AcceptPart(this, Items, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => RecordSymbol.GetSymbolLength() +
                FieldList.GetSymbolLength() +
                VariantSection.GetSymbolLength() +
                Items.GetSymbolLength() +
                EndSymbol.GetSymbolLength();
    }
}