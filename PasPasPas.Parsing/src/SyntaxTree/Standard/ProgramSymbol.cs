using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     syntax tree for a program
    /// </summary>
    public class Program : StandardSyntaxTreeBase {

        /// <summary>
        ///     main block
        /// </summary>
        public BlockSymbol MainBlock { get; set; }

        /// <summary>
        ///     program header
        /// </summary>
        public ISyntaxPart ProgramHead { get; set; }

        /// <summary>
        ///     program name
        /// </summary>
        public NamespaceName ProgramName
            => (ProgramHead as ProgramHeadSymbol)?.Name;

        /// <summary>
        ///     uses list
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
        ///     symbol length
        /// </summary>
        public int Length
            => ProgramHead.Length + Uses.Length + MainBlock.Length + Dot.Length;

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