using PasPasPas.Globals.Files;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     library definition
    /// </summary>
    public class LibrarySymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     library symbol
        /// </summary>
        /// <param name="libraryHead"></param>
        /// <param name="uses"></param>
        /// <param name="mainBlock"></param>
        /// <param name="dot"></param>
        /// <param name="filePath"></param>
        public LibrarySymbol(LibraryHeadSymbol libraryHead, UsesFileClauseSymbol uses, BlockSymbol mainBlock, Terminal dot, FileReference filePath) {
            LibraryHead = libraryHead;
            Uses = uses;
            MainBlock = mainBlock;
            Dot = dot;
            FilePath = filePath;
        }

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationListSymbol Hints
            => LibraryHead?.Hints as HintingInformationListSymbol;

        /// <summary>
        ///     library head
        /// </summary>
        public LibraryHeadSymbol LibraryHead { get; }

        /// <summary>
        ///     library name
        /// </summary>
        public NamespaceNameSymbol LibraryName
            => LibraryHead?.LibraryName;

        /// <summary>
        ///     main block
        /// </summary>
        public BlockSymbol MainBlock { get; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public UsesFileClauseSymbol Uses { get; }

        /// <summary>
        ///     file path
        /// </summary>
        public FileReference FilePath { get; }

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal Dot { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, LibraryHead, visitor);
            AcceptPart(this, Uses, visitor);
            AcceptPart(this, MainBlock, visitor);
            AcceptPart(this, Dot, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => LibraryHead.GetSymbolLength() +
                Uses.GetSymbolLength() +
                MainBlock.GetSymbolLength() +
                Dot.GetSymbolLength();


    }
}
