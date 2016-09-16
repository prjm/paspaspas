namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     type alias definition
    /// </summary>
    public class TypeAliasDefinition : SyntaxPartBase {

        /// <summary>
        ///     generic type suffix
        /// </summary>
        public GenericTypeSuffix GenericSuffix { get; set; }

        /// <summary>
        ///     source type name
        /// </summary>
        public NamespaceName TypeName { get; set; }

    }
}