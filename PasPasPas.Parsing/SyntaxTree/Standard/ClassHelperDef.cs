namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class helper declaration
    /// </summary>
    public class ClassHelperDef : SyntaxPartBase {

        /// <summary>
        ///     items
        /// </summary>
        public ClassHelperItems HelperItems { get; set; }

        /// <summary>
        ///     class parent
        /// </summary>
        public ParentClass ClassParent { get; set; }

        /// <summary>
        ///     class helper name
        /// </summary>
        public NamespaceName HelperName { get; set; }

    }
}