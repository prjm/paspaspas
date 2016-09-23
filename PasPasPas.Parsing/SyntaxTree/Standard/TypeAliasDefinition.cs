namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     type alias definition
    /// </summary>
    public class TypeAliasDefinition : SyntaxPartBase {

        /// <summary>
        ///     generic type suffix
        /// </summary>
        public GenericPostfix GenericSuffix { get; set; }

        /// <summary>
        ///     source type name
        /// </summary>
        public NamespaceName TypeName { get; set; }

    }
}