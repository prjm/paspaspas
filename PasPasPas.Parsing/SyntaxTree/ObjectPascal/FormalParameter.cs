namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     formal parameter
    /// </summary>
    public class FormalParameter : SyntaxPartBase {

        /// <summary>
        ///     user attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     default value
        /// </summary>
        public Expression DefaultValue { get; set; }

        /// <summary>
        ///     parse a list of identifiers
        /// </summary>
        public IdentList ParamNames { get; set; }

        /// <summary>
        ///     parameter typs
        /// </summary>
        public int ParamType { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDeclaration { get; set; }

    }
}