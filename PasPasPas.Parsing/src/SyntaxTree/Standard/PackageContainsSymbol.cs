using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     contains clause
    /// </summary>
    public class PackageContainsSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new package contains clause
        /// </summary>
        /// <param name="containsSymbol"></param>
        /// <param name="containsList"></param>
        /// <param name="semicolon">semicolon</param>
        public PackageContainsSymbol(Terminal containsSymbol, NamespaceFileNameListSymbol containsList, Terminal semicolon) {
            ContainsSymbol = containsSymbol;
            ContainsList = containsList;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     contained units
        /// </summary>
        public NamespaceFileNameListSymbol ContainsList { get; }

        /// <summary>
        ///     contains symbol
        /// </summary>
        public Terminal ContainsSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ContainsSymbol, visitor);
            AcceptPart(this, ContainsList, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ContainsSymbol.GetSymbolLength() +
                ContainsList.GetSymbolLength() +
                Semicolon.GetSymbolLength();

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }
    }
}