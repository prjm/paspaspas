using System;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     tokenizer based on a character class
    /// </summary>
    public abstract class CharacterClassTokenGroupValue : PatternContinuation {

        /// <summary>
        ///     create a new character class token group value
        /// </summary>
        /// <param name="tokenId"></param>
        public CharacterClassTokenGroupValue(int tokenId, int minLength) {
            TokenId = tokenId;
            MinLength = minLength;
        }

        /// <summary>
        ///     token kind
        /// </summary>
        public int TokenId { get; }

        /// <summary>
        ///     minimal length
        /// </summary>
        protected int MinLength { get; }

        /// <summary>
        ///     error message
        /// </summary>
        protected virtual Guid MinLengthMessage
            => TokenizerBase.UnexpectedEndOfToken;

        /// <summary>
        ///     test if a character matches the given class
        /// </summary>
        /// <param name="input">char to test</param>
        /// <returns></returns>
        protected abstract bool MatchesClass(char input);

        /// <summary>
        ///     parse the complete token
        /// </summary>
        public override Token Tokenize(ITokenizerState state) {

            if (!state.AtEof) {
                var currentChar = state.NextChar(false);
                while (MatchesClass(currentChar)) {
                    state.Append(currentChar);

                    if (state.AtEof)
                        break;
                    else
                        currentChar = state.NextChar(false);
                }

                if (MatchesClass(currentChar))
                    state.Append(currentChar);
            }

            if (MinLength > 0 && state.Length < MinLength)
                state.Error(MinLengthMessage);

            if (state.KeepTokenValue(TokenId))
                return new Token(TokenId, state);
            else
                return new Token(TokenId, state.CurrentPosition, string.Empty);
        }


    }

}