namespace PasPasPas.Api {

    /// <summary>
    ///     interface for parser input
    /// </summary>
    public interface IParserInput {

        /// <summary>
        ///     tests if any input is left
        /// </summary>
        /// <returns><c>true</c> if some input is left</returns>
        bool AtEof();

        /// <summary>
        ///     get the next char from the input
        /// </summary>
        /// <returns>next char</returns>
        char NextChar();

        /// <summary>
        ///     put back a character
        /// </summary>
        /// <param name="valueToPutback">character to put back</param>
        void Putback(char valueToPutback);
    }
}
