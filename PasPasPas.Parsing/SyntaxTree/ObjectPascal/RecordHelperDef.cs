namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     record helper
    /// </summary>
    public class RecordHelperDef : SyntaxPartBase {

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