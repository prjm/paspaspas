#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     a pattern continuation is used as a base class to parse some type of token
    /// </summary>
    public abstract class PatternContinuation {

        /// <summary>
        ///     generate the next token
        /// </summary>
        public abstract Token Tokenize(TokenizerState state);

        /// <summary>
        ///     test if a given character can trigger a new line
        /// </summary>
        /// <param name="value">char to test</param>
        /// <returns><c>true</c> if the character can trigger a new line</returns>
        public static bool IsNewLineChar(char value)
            => value == '\r' || value == '\n';

        /// <summary>
        ///     test if a string contains a newline character
        /// </summary>
        /// <param name="value">string to tet</param>
        /// <returns><c>true</c> if it contains a new line character</returns>
        public static bool ContainsNewLineChar(string value) {
            for (var index = 0; index < value.Length; index++) {
                if (IsNewLineChar(value[index]))
                    return true;
            }
            return false;
        }
    }
}