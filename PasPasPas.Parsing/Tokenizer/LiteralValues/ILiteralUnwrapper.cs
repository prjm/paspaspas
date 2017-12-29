namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     helper for unwrapping literals
    /// </summary>
    public interface ILiteralUnwrapper {

        /// <summary>
        ///     unwrap a literal string value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string UnwrapString(object value);

    }
}
