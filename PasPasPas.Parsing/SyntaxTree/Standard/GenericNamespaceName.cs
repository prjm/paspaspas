namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespace name with generuc suffix
    /// </summary>
    public class GenericNamespaceName : SyntaxPartBase {

        /// <summary>
        ///     generic part
        /// </summary>
        public GenericPostfix GenericPart { get; set; }

        /// <summary>
        ///     namespace name
        /// </summary>
        public NamespaceName Name { get; set; }
    }
}
