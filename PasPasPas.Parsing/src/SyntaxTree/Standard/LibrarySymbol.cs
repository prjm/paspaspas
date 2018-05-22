using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     library definition
    /// </summary>
    public class LibrarySymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hints
            => LibraryHead?.Hints as HintingInformationList;

        /// <summary>
        ///     library head
        /// </summary>
        public LibraryHeadSymbol LibraryHead { get; set; }

        /// <summary>
        ///     library name
        /// </summary>
        public NamespaceName LibraryName
            => LibraryHead?.LibraryName;

        /// <summary>
        ///     main block
        /// </summary>
        public BlockSymbol MainBlock { get; set; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public ISyntaxPart Uses { get; set; }

        /// <summary>
        ///     file path
        /// </summary>
        public IFileReference FilePath { get; set; }

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal Dot { get; set; }

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
        public int Length
            => LibraryHead.Length + Uses.Length + MainBlock.Length + Dot.Length;


    }
}
