namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     class for invalid tokens
    /// </summary>
    public class InvalidToken : Terminal {

        /// <summary>
        ///     create a new invalid token
        /// </summary>
        /// <param name="token"></param>
        public InvalidToken(Token token) : base(token) { }
    }
}