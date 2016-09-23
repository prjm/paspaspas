namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     type name / reference to a type
    /// </summary>
    public class TypeName : SyntaxPartBase {
        public GenericPostfix GenericType { get; internal set; }

        /// <summary>
        ///     type id
        /// </summary>
        public NamespaceName TypeId { get; set; }
    }
}
