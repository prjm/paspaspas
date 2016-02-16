using PasPasPas.Api.Input;
using System.Collections.Generic;
using System.Text;

namespace PasPasPas.Internal.Input {

    /// <summary>
    ///     base class for file input
    /// </summary>
    public abstract class InputBase : IParserInput {

        private Stack<char> putbackBuffer
            = new Stack<char>(1024);

        /// <summary>
        ///     check if the end of the source file is reached
        /// </summary>
        protected abstract bool IsSourceAtEof { get; }

        /// <summary>
        ///     gets the next char from the source
        /// </summary>
        protected abstract char NextCharFromSource();

        /// <summary>
        ///     test if the end of input is reached
        /// </summary>
        public bool AtEof
            => (putbackBuffer.Count < 1) && (IsSourceAtEof);

        /// <summary>
        ///     get the next char
        /// </summary>
        /// <returns>next character</returns>
        public char NextChar() {
            if (putbackBuffer.Count > 0)
                return putbackBuffer.Pop();
            else
                return NextCharFromSource();
        }

        /// <summary>
        ///     put back an entire string
        /// </summary>
        /// <param name="buffer"></param>
        public void Putback(string buffer) {
            for (int charIndex = buffer.Length - 1; charIndex >= 0; charIndex--) {
                putbackBuffer.Push(buffer[charIndex]);
            }
        }

        /// <summary>
        ///     put back an entire stringbuffer
        /// </summary>
        /// <param name="stringToPutback"></param>
        public void Putback(StringBuilder stringToPutback) {
            for (int charIndex = stringToPutback.Length - 1; charIndex >= 0; charIndex--) {
                putbackBuffer.Push(stringToPutback[charIndex]);
            }
            stringToPutback.Clear();
        }



        /// <summary>
        ///     putback a single char
        /// </summary>
        /// <param name="valueToPutback"></param>
        public void Putback(char valueToPutback) {
            putbackBuffer.Push(valueToPutback);
        }
    }
}
