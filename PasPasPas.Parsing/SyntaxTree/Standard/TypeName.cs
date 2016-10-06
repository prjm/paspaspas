namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     type name / reference to a type
    /// </summary>
    public class TypeName : SyntaxPartBase {

        /// <summary>
        ///     string type
        /// </summary>
        public int StringType { get; set; }
            = TokenKind.Undefined;

    }
}
