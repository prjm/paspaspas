using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     library head
    /// </summary>
    public class LibraryHeadSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     library head symbol
        /// </summary>
        /// <param name="librarySymbol"></param>
        /// <param name="libraryName"></param>
        /// <param name="hints"></param>
        /// <param name="semicolon"></param>
        public LibraryHeadSymbol(Terminal librarySymbol, NamespaceNameSymbol libraryName, HintingInformationListSymbol hints, Terminal semicolon) {
            LibrarySymbol = librarySymbol;
            LibraryName = libraryName;
            Hints = hints;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     hints
        /// </summary>
        public ISyntaxPart Hints { get; }

        /// <summary>
        ///     library name
        /// </summary>
        public NamespaceNameSymbol LibraryName { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     library
        /// </summary>
        public Terminal LibrarySymbol { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => LibrarySymbol.GetSymbolLength() +
                LibraryName.GetSymbolLength() +
                Hints.GetSymbolLength() +
                Semicolon.GetSymbolLength();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, LibrarySymbol, visitor);
            AcceptPart(this, LibraryName, visitor);
            AcceptPart(this, Hints, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }


    }
}