namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

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
