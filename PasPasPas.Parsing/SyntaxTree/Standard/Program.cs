using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     syntax tree for a program
    /// </summary>
    public class Program : StandardSyntaxTreeBase {

        /// <summary>
        ///     file path
        /// </summary>
        public IFileReference FilePath
            => LastTerminalToken?.FilePath;

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
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }

    }
}
