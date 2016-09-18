namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     simple field declaration
    /// </summary>
    public class ClassField : SyntaxPartBase {

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hint { get; set; }

        /// <summary>
        ///     names
        /// </summary>
        public IdentifierList Names { get; set; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDecl { get; set; }

    }
}