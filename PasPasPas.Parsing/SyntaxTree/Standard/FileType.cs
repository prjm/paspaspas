namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     file type
    /// </summary>
    public class FileType : SyntaxPartBase {

        /// <summary>
        ///     optional subtype
        /// </summary>
        public TypeSpecification TypeDefinition { get; set; }

    }
}