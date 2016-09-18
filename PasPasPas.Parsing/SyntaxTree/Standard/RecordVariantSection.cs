namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variant section
    /// </summary>
    public class RecordVariantSection : SyntaxPartBase {

        /// <summary>
        ///     name of the variant
        /// </summary>
        public Identifier Name { get; set; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDeclaration { get; set; }

    }
}