namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     class for invalid tokens
    /// </summary>
    public class InvalidToken : Terminal {

        /// <summary>
        ///     create a new invalid token
        /// </summary>
        /// <param name="pascalToken"></param>
        public InvalidToken(PascalToken pascalToken) : base(pascalToken) { }
    }
}