namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     interface to convert real values
    /// </summary>
    public interface IRealConverter {

        /// <summary>
        ///     convert integer literals to one real literal
        /// </summary>
        /// <param name="digits">digit</param>
        /// <param name="decimals">decimal places</param>
        /// <param name="minus">minus sign</param>
        /// <param name="exponent">exponent</param>
        /// <returns>real literal value</returns>
        object Convert(object digits, object decimals, bool minus, object exponent);
    }
}
