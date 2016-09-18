namespace PasPasPas.Parsing.SyntaxTree.Standard {

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