namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     generic defnition part
    /// </summary>
    public class GenericDefinitionPart : SyntaxPartBase {

        /// <summary>
        ///     parse identifiert
        /// </summary>
        public PascalIdentifier Identifier { get; set; }

    }
}