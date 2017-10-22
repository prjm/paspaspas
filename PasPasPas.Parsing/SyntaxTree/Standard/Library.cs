using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     library definition
    /// </summary>
    public class Library : StandardSyntaxTreeBase {

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
        ///     file path
        /// </summary>
        public IFileReference FilePath { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}
