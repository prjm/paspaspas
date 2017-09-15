using System;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     tokenizer based on a character class
    /// </summary>
    public class CharacterClassTokenGroupValue : PatternContinuation {

        /// <summary>
        ///     create a new character class token group value
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="charClass">character class</param>
        public CharacterClassTokenGroupValue(int tokenId, CharacterClass charClass, int minLength = 0)
            : this(tokenId, charClass, minLength, Guid.Empty) { }


        /// <summary>
        ///     create a new character class token group value
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="charClass">character class</param>
        public CharacterClassTokenGroupValue(int tokenId, CharacterClass charClass, int minLength, Guid parser) {
            TokenId = tokenId;
            MinLength = minLength;
            CharClass = charClass;
        }

        /// <summary>
        ///     token kind
        /// </summary>
        public int TokenId { get; }

        /// <summary>
        ///     matching character class
        /// </summary>
        public CharacterClass CharClass { get; }

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
        protected bool MatchesClass(char input)
            => CharClass.Matches(input);

        /// <summary>
        ///     parse the complete token
        /// </summary>
        public override Token Tokenize(TokenizerState state) {

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

            if (TokenId == TokenKind.HexNumber) {
                var value = state.GetBufferContent().Pool();
                return new Token(TokenId, state.CurrentPosition, value, Literals.ParseHexNumberLiteral(value));
            }
            else if (state.KeepTokenValue(TokenId))
                return new Token(TokenId, state);
            else
                return new Token(TokenId, state.CurrentPosition, string.Empty);
        }


    }

}