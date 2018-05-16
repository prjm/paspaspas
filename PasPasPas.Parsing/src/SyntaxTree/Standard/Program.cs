using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     syntax tree for a program
    /// </summary>
    public class Program : StandardSyntaxTreeBase {

        /// <summary>
        ///     main block
        /// </summary>
        public Block MainBlock { get; set; }

        /// <summary>
        ///     program header
        /// </summary>
        public ProgramHead ProgramHead { get; set; }

        /// <summary>
        ///     program name
        /// </summary>
        public NamespaceName ProgramName
            => ProgramHead?.Name;

        /// <summary>
        ///     uses list
        /// </summary>
        public UsesFileClause Uses { get; set; }

        /// <summary>
        ///     file path
        /// </summary>
        public IFileReference FilePath { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
