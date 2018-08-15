using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     package definition
    /// </summary>
    public class Package : StandardSyntaxTreeBase {


        /// <summary>
        ///     contans clause
        /// </summary>
        public PackageContains ContainsClause { get; set; }

        /// <summary>
        ///     package head
        /// </summary>
        public PackageHead PackageHead { get; set; }

        /// <summary>
        ///     package name
        /// </summary>
        public NamespaceNameSymbol PackageName
            => PackageHead?.PackageName;

        /// <summary>
        ///     requires clause
        /// </summary>
        public PackageRequires RequiresClause { get; set; }

        /// <summary>
        ///     path
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
