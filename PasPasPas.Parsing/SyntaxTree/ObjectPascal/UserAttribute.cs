namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     user defined attribute (rtti)
    /// </summary>
    public class UserAttribute : SyntaxPartBase {

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