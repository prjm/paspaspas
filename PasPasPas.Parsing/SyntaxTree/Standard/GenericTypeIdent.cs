namespace PasPasPas.Parsing.SyntaxTree.Standard {

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
        public Identifier Identifier { get; set; }

    }
}