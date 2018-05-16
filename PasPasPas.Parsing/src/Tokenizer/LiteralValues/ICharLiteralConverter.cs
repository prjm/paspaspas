namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     converts an integer literal to a character literal
    /// </summary>
    public interface ICharLiteralConverter {

        /// <summary>
        ///     convert the literal
        /// </summary>
        /// <param name="literal">literal value</param>
        /// <returns>character value</returns>
        object Convert(object literal);
    }
}
