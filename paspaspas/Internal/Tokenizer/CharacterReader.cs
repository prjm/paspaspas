using PasPasPas.Api;
using PasPasPas.Internal.Log;
using System.Text;
using System;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     base class for char-based tokenizing
    /// </summary>
    public abstract class CharacterTokenizer : ObjectWithLog {

        /// <summary>
        ///     tests if the tokenizer matches
        /// </summary>
        /// <param name="input">parser input</param>
        /// <returns></returns>
        public abstract bool Matches(FileScanner input);

        /// <summary>
        ///     parse and generate token
        /// </summary>
        /// <returns></returns>
        public abstract PascalToken Parse(FileScanner input);

        /// <summary>
        ///     test if a sequence is matched
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="sequence">sequence to match</param>
        /// <returns></returns>
        protected static bool MatchesSequence(FileScanner input, string sequence) {
            if (input.AtEof) {
                return false;
            }

            input.Mark();
            int index;
            for (index = 0; index < sequence.Length & (!input.AtEof); index++) {
                var currentChar = input.NextChar();
                if (currentChar != sequence[index])
                    break;
            }

            if (index != sequence.Length) {
                input.Reset();
                return false;
            }

            input.Reset();
            return true;
        }

        /// <summary>
        ///     tests if the string buillder ends with a given string
        /// </summary>
        /// <param name="result">result</param>
        /// <param name="sequenceEnd">string to search for</param>
        /// <returns></returns>
        protected static bool EndsWith(StringBuilder result, string sequenceEnd) {
            if (sequenceEnd.Length > result.Length)
                return false;

            for (int i = result.Length - sequenceEnd.Length; i < result.Length; i++) {
                if (result[i] != sequenceEnd[i - (result.Length - sequenceEnd.Length)])
                    return false;
            }

            return true;
        }

        /// <summary>
        ///      read until a specific sequence is encountered
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="sequenceEnd">sequence end</param>
        /// <param name="tokenKind">token kind</param>
        /// <returns>generated token</returns>
        protected PascalToken ReadUntilSequence(FileScanner input, string sequenceEnd, int tokenKind) {
            input.Mark();
            StringBuilder result = new StringBuilder();
            while (!(input.AtEof) && (!EndsWith(result, sequenceEnd))) {
                result.Append(input.NextChar());
            }

            return new PascalToken() {
                Kind = tokenKind,
                Value = result.ToString()
            };
        }

    }

    /// <summary>
    ///     reads preprocessor tokens
    /// </summary>
    public class PreprocessorTokenizer : CharacterTokenizer {

        /// <summary>
        ///     test if the input matches a preprocessor directive
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>sequence to match</returns>
        public override bool Matches(FileScanner input)
            => MatchesSequence(input, "{$");

        /// <summary>
        ///     read a preprocessor directive
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>token</returns>
        public override PascalToken Parse(FileScanner input)
            => ReadUntilSequence(input, "}", PascalToken.Preprocessor);
    }

    /// <summary>
    ///     reads whitspace
    /// </summary>
    public class WhitespaceReader : CharacterTokenizer {

        /// <summary>
        ///     test if the current character is whitespacew
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        public override bool Matches(FileScanner input) {
            input.Mark();
            var result = char.IsWhiteSpace(input.NextChar());
            input.Reset();
        }

        public override PascalToken Parse(FileScanner input) {
            throw new NotImplementedException();
        }
    }

}
