using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     package definition
    /// </summary>
    public class Package : SyntaxPartBase {

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

    }
}
