namespace PasPasPas.Infrastructure.Internal.Input {

    /// <summary>
    ///     parser input for <c>strings</c>
    /// </summary>
    public class StringInput : InputBase {

        private readonly string input;
        private int position = 0;

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
        protected override bool IsSourceAtEof
                => (position >= input.Length);

        /// <summary>
        ///     get the next char
        /// </summary>
        /// <returns></returns>
        protected override char NextCharFromSource() {
            char result = input[position];
            position++;
            return result;
        }
    }
}
