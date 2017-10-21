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

        /// <summary>
        ///     unwrap an integer value
        /// </summary>
        /// <param name="parsedValue">value to unwrap</param>
        /// <returns></returns>
        ulong UnwrapInteger(object parsedValue);

        /// <summary>
        ///     unwrap a hexadecimal value
        /// </summary>
        /// <param name="parsedValue"></param>
        /// <returns></returns>
        ulong UnwrapHexnumber(object parsedValue);
    }
}
