using System.Collections.Generic;

namespace PasPasPas.Api {

    /// <summary>
    ///     parser input for <c>strings</c>
    /// </summary>
    public class StringInput : IParserInput {

        private readonly string input;
        private int position = 0;
        private Stack<char> putbackChars = new Stack<char>();

        /// <summary>
        ///     creates a new string input
        /// </summary>
        /// <param name="input">string input</param>
        public StringInput(string input) {
            this.input = input;
        }

        /// <summary>
        ///     checks if eof is reached
        /// </summary>
        /// <returns><c>true</c> if the end of string is reached</returns>
        public bool AtEof()
            => (position >= input.Length) && (putbackChars.Count < 1);

        /// <summary>
        ///     get the next char
        /// </summary>
        /// <returns></returns>
        public char NextChar() {
            if (putbackChars.Count > 0)
                return putbackChars.Pop();

            char result = input[position];
            position++;
            return result;
        }

        /// <summary>
        ///     put a char back into the input
        /// </summary>
        /// <param name="valueToPutback">char to putback</param>
        public void Putback(char valueToPutback) {
            putbackChars.Push(valueToPutback);
        }
    }
}
