namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     type declaration
    /// </summary>
    public class TypeDeclaration : SyntaxPartBase {

        /// <summary>
        ///     user attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     hinting directives
        /// </summary>
        public HintingInformationList Hint { get; set; }

        /// <summary>
        ///     type speicifcaiton
        /// </summary>
        public TypeSpecification TypeSpecification { get; set; }

        /// <summary>
        ///     type id
        /// </summary>
        public GenericTypeIdentifier TypeId { get; set; }

    }
}