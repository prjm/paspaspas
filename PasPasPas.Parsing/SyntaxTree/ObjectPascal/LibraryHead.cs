namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     library head
    /// </summary>
    public class LibraryHead : SyntaxPartBase {

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hints { get; set; }

        /// <summary>
        ///     library name
        /// </summary>
        public NamespaceName Name { get; set; }

    }
}