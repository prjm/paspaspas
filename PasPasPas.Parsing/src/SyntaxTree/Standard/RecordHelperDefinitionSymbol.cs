using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record helper
    /// </summary>
    public class RecordHelperDefinitionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new record helper symbol
        /// </summary>
        /// <param name="recordSymbol"></param>
        /// <param name="helperSymbol"></param>
        /// <param name="forSymbol"></param>
        /// <param name="name"></param>
        /// <param name="items"></param>
        /// <param name="endSymbol"></param>
        public RecordHelperDefinitionSymbol(Terminal recordSymbol, Terminal helperSymbol, Terminal forSymbol, TypeNameSymbol name, RecordHelperItemsSymbol items, Terminal endSymbol) {
            RecordSymbol = recordSymbol;
            HelperSymbol = helperSymbol;
            ForSymbol = forSymbol;
            Name = name;
            Items = items;
            EndSymbol = endSymbol;
        }

        /// <summary>
        ///     record helper items
        /// </summary>
        public RecordHelperItemsSymbol Items { get; }

        /// <summary>
        ///     record helper name
        /// </summary>
        public TypeNameSymbol Name { get; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }

        /// <summary>
        ///     for symbol
        /// </summary>
        public Terminal ForSymbol { get; }

        /// <summary>
        ///     helper symbol
        /// </summary>
        public Terminal HelperSymbol { get; }

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
            AcceptPart(this, HelperSymbol, visitor);
            AcceptPart(this, ForSymbol, visitor);
            AcceptPart(this, Name, visitor);
            AcceptPart(this, Items, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => RecordSymbol.GetSymbolLength() +
                HelperSymbol.GetSymbolLength() +
                ForSymbol.GetSymbolLength() +
                Name.GetSymbolLength() +
                Items.GetSymbolLength() +
                EndSymbol.GetSymbolLength();
    }
}