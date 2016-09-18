namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     user defined attribute (rtti)
    /// </summary>
    public class UserAttributeDefinition : SyntaxPartBase {

        /// <summary>
        ///     üaraparameter expressions
        /// </summary>
        public ExpressionList Expressions { get; set; }

        /// <summary>
        ///     name of the attribute
        /// </summary>
        public NamespaceName Name { get; set; }

    }
}