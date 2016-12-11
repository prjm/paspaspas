namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formal parameter
    /// </summary>
    public class FormalParameter : SyntaxPartBase {

        /// <summary>
        ///     parameter attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     default value
        /// </summary>
        public Expression DefaultValue { get; set; }

        /// <summary>
        ///     parse a list of identifiers
        /// </summary>
        public IdentifierList ParameterNames { get; set; }

        /// <summary>
        ///     parameter typs
        /// </summary>
        public int ParameterType { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDeclaration { get; set; }

    }
}