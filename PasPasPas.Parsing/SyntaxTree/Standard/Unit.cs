using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     pascal unit
    /// </summary>
    public class Unit : SyntaxPartBase {

        /// <summary>
        ///     file path
        /// </summary>
        public IFileReference FilePath
            => UnitHead?.FirstTerminalToken?.FilePath;

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hints
            => UnitHead?.Hint;

        /// <summary>
        ///     unit block
        /// </summary>
        public UnitBlock UnitBlock { get; set; }

        /// <summary>
        ///     unit head section
        /// </summary>
        public UnitHead UnitHead { get; set; }

        /// <summary>
        ///     unit implementation section
        /// </summary>
        public UnitImplementation UnitImplementation { get; set; }

        /// <summary>
        ///     unit interface
        /// </summary>
        public UnitInterface UnitInterface { get; set; }

        /// <summary>
        ///     unit name
        /// </summary>
        public NamespaceName UnitName
            => UnitHead?.UnitName;
    }
}
