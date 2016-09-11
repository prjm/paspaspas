namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     method declaration
    /// </summary>
    public class MethodDecl : SyntaxPartBase {

        /// <summary>
        ///     user attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     <c>true</c> if class method
        /// </summary>
        public bool Class { get; set; }

        /// <summary>
        ///     method directives
        /// </summary>
        public MethodDirectives Directives { get; set; }

        /// <summary>
        ///     method heading
        /// </summary>
        public MethodDeclHeading Heading { get; set; }

        /// <summary>
        ///     method implementation
        /// </summary>
        public Block MethodBody { get; set; }

    }
}
