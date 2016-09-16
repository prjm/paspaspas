namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     variant section
    /// </summary>
    public class RecordVariantSection : SyntaxPartBase {

        /// <summary>
        ///     name of the variant
        /// </summary>
        public PascalIdentifier Name { get; set; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDecl { get; set; }

    }
}