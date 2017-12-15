namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     simple integer parser
    /// </summary>
    public interface IIntegerLiteralParser {

        /// <summary>
        ///     parse a given literal
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>parsed literal</returns>
        object Parse(string input);

        /// <summary>
        ///     convert a number to a literal
        /// </summary>
        /// <param name="number">number to convert</param>
        /// <returns>literal object</returns>
        object ToLiteral(ulong number);
    }
}
