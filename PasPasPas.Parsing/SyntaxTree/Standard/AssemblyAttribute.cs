namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly attribute
    /// </summary>
    public class AssemblyAttributeDeclaration : SyntaxPartBase {

        /// <summary>
        ///     attribute definition
        /// </summary>
        public UserAttributeDefinition Attribute { get; set; }

    }
}
