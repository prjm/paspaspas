namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formal parameter
    /// </summary>
    public class FormalParameter : SyntaxPartBase {

        /// <summary>
        ///     parameter attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public Identifier ParameterName { get; set; }

        /// <summary>
        ///     parameter type (var, const, out)
        /// </summary>
        public int ParameterType { get; set; }
            = TokenKind.Undefined;

    }
}