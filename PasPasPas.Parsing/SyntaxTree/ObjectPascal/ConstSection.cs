namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     section for constants
    /// </summary>
    public class ConstSection : SyntaxPartBase {

        /// <summary>
        ///     kind of constant
        /// </summary>
        public int Kind { get; set; }
                = TokenKind.Undefined;

    }
}
