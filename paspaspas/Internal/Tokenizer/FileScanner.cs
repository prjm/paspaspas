using PasPasPas.Api.Input;
using System.Text;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     a simple helper for tokenizing an input
    /// </summary>
    public class FileScanner {

        /// <summary>
        ///     scanner input
        /// </summary>
        public IParserInput Input { get; set; }

        /// <summary>
        ///     test if the end of input is reached
        /// </summary>

        public bool AtEof
            => Input.AtEof;

        private StringBuilder buffer
            = new StringBuilder();

        /// <summary>
        ///     mark the start of the current token
        /// </summary>
        public void Mark() {
            buffer.Clear();
        }

        /// <summary>
        ///     return to the last mark
        /// </summary>
        public void Reset() {
            Input.Putback(buffer);
        }

        /// <summary>
        ///     get the next char from the input file
        /// </summary>
        /// <returns></returns>
        public char NextChar() {
            char result = Input.NextChar();
            buffer.Append(result);
            return result;
        }

    }
}
