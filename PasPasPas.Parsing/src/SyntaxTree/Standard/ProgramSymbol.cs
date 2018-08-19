using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     syntax tree for a program
    /// </summary>
    public class ProgramSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new program head
        /// </summary>
        /// <param name="programHead"></param>
        /// <param name="uses"></param>
        /// <param name="mainBlock"></param>
        /// <param name="dot"></param>
        /// <param name="filePath"></param>
        public ProgramSymbol(ProgramHeadSymbol programHead, UsesFileClause uses, BlockSymbol mainBlock, Terminal dot, IFileReference filePath) {
            ProgramHead = programHead;
            Uses = uses;
            MainBlock = mainBlock;
            Dot = dot;
            FilePath = filePath;
        }

        /// <summary>
        ///     main block
        /// </summary>
        public BlockSymbol MainBlock { get; }

        /// <summary>
        ///     program header
        /// </summary>
        public ProgramHeadSymbol ProgramHead { get; }

        /// <summary>
        ///     program name
        /// </summary>
        public NamespaceNameSymbol ProgramName
            => (ProgramHead as ProgramHeadSymbol)?.Name;

        /// <summary>
        ///     uses list
        /// </summary>
        public UsesFileClause Uses { get; }

        /// <summary>
        ///     file path
        /// </summary>
        public IFileReference FilePath { get; }

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal Dot { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ProgramHead.GetSymbolLength() +
                Uses.GetSymbolLength() +
                MainBlock.GetSymbolLength() +
                Dot.GetSymbolLength();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ProgramHead, visitor);
            AcceptPart(this, Uses, visitor);
            AcceptPart(this, MainBlock, visitor);
            AcceptPart(this, Dot, visitor);
            visitor.EndVisit(this);
        }
    }
}