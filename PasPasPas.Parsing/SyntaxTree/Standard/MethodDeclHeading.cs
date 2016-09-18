namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     method declaration heading
    /// </summary>
    public class MethodDeclarationHeading : SyntaxPartBase {

        /// <summary>
        ///     generic definition
        /// </summary>
        public GenericDefinition GenericDefinition { get; set; }

        /// <summary>
        ///     method kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     method name
        /// </summary>
        public NamespaceName Name { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; set; }

        /// <summary>
        ///     result type
        /// </summary>
        public TypeSpecification ResultType { get; set; }

        /// <summary>
        ///     result type attributes
        /// </summary>
        public UserAttributes ResultTypeAttributes { get; set; }
    }
}