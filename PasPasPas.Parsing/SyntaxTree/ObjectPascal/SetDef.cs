namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     a set definition
    /// </summary>
    public class SetDef : SyntaxPartBase {

        /// <summary>
        ///     inner type reference
        /// </summary>
        public TypeSpecification TypeDefinition { get; set; }

    }
}