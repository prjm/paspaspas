namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     type name / reference to a type
    /// </summary>
    public class TypeName : SyntaxPartBase {

        /// <summary>
        ///     generic type postfix
        /// </summary>
        public GenericPostfix GenericType { get; set; }

        /// <summary>
        ///     string type
        /// </summary>
        public int StringType { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     type id
        /// </summary>
        public NamespaceName TypeId { get; set; }
    }
}
