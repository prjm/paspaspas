namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record helper
    /// </summary>
    public class RecordHelperDefinition : SyntaxPartBase {

        /// <summary>
        ///     record helper items
        /// </summary>
        public RecordHelperItems Items { get; set; }

        /// <summary>
        ///     record helper name
        /// </summary>
        public NamespaceName Name { get; set; }

    }
}