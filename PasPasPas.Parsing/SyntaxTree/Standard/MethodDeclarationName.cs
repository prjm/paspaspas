namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///    method declaration names
    /// </summary>
    public class MethodDeclarationName : SyntaxPartBase {

        /// <summary>
        ///     namespace name
        /// </summary>
        public NamespaceName Name { get; set; }

        /// <summary>
        ///     generic parameters
        /// </summary>
        public GenericDefinition GenericDefinition { get; set; }
    }
}
