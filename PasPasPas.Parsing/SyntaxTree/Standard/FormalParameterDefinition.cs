namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formal definition parameter
    /// </summary>
    public class FormalParameterDefinition : SyntaxPartBase {

        /// <summary>
        ///     default value
        /// </summary>
        public Expression DefaultValue { get; internal set; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDeclaration { get; internal set; }
    }
}
