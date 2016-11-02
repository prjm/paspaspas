using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     syntax tree for a program
    /// </summary>
    public class Program : SyntaxPartBase {

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
    }
}
