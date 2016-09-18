namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     a set definition
    /// </summary>
    public class SetDefinition : SyntaxPartBase {

        /// <summary>
        ///     inner type reference
        /// </summary>
        public TypeSpecification TypeDefinition { get; set; }

    }
}