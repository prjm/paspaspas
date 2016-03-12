using System.Text;

namespace PasPasPas.Api.Input {

    /// <summary>
    ///     interface for parser input
    /// </summary>
    public interface IParserInput {

        /// <summary>
        ///     tests if any input is left
        /// </summary>
        /// <returns><c>true</c> if some input is left</returns>
        bool AtEof { get; }

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

        /// <summary>
        ///     put back a string
        /// </summary>
        /// <param name="valueToPutback"></param>
        void Putback(string valueToPutback);

        /// <summary>
        ///     putback a buffer structur
        /// </summary>
        /// <param name="buffer"></param>
        void Putback(StringBuilder buffer);
    }
}
