namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     generic type ident
    /// </summary>
    public class GenericTypeIdentifier : SyntaxPartBase {

        /// <summary>
        ///     generic definition
        /// </summary>
        public GenericDefinition GenericDefinition { get; set; }

        /// <summary>
        ///     type name
        /// </summary>
        public PascalIdentifier Identifier { get; set; }

    }
}