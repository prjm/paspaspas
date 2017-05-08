using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     package definition
    /// </summary>
    public class Package : StandardSyntaxTreeBase {

        public Package(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     contans clause
        /// </summary>
        public PackageContains ContainsClause { get; set; }

        /// <summary>
        ///     package file path
        /// </summary>
        public IFileReference FilePath
            => PackageHead?.FirstTerminalToken?.FilePath;

        /// <summary>
        ///     package head
        /// </summary>
        public PackageHead PackageHead { get; set; }

        /// <summary>
        ///     package name
        /// </summary>
        public NamespaceName PackageName
            => PackageHead?.PackageName;

        /// <summary>
        ///     requires clause
        /// </summary>
        public PackageRequires RequiresClause { get; set; }

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
