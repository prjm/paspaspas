namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     Token kinds
    /// </summary>
    public static class TokenKind {


        /// <summary>
        ///     undefined tokens
        /// </summary>
        public const int Undefined = 1;

        /// <summary>
        ///     end-of-input
        /// </summary>
        public const int Eof = 2;

        /// <summary>
        ///     preprocessor token
        /// </summary>
        public const int Preprocessor = 5;

        /// <summary>
        ///     comment
        /// </summary>
        public const int Comment = 6;


    }
}
