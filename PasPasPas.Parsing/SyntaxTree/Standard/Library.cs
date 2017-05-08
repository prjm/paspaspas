using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     library definition
    /// </summary>
    public class Library : StandardSyntaxTreeBase {

        public Library(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     file path
        /// </summary>
        public IFileReference FilePath
            => LibraryHead?.FirstTerminalToken?.FilePath;

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hints
            => LibraryHead?.Hints;

        /// <summary>
        ///     library head
        /// </summary>
        public LibraryHead LibraryHead { get; set; }

        /// <summary>
        ///     library name
        /// </summary>
        public NamespaceName LibraryName
            => LibraryHead?.LibraryName;

        /// <summary>
        ///     main block
        /// </summary>
        public Block MainBlock { get; set; }

        /// <summary>
        ///     uses clause
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
