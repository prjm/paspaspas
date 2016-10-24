using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     library definition
    /// </summary>
    public class Library : SyntaxPartBase {

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


    }
}
