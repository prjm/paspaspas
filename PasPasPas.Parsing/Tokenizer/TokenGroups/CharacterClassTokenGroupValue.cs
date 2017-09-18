using System;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.LiteralValues;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     tokenizer based on a character class
    /// </summary>
    public sealed class CharacterClassTokenGroupValue : PatternContinuation {

        /// <summary>
        ///     create a new character class token group value
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="charClass">character class</param>
        public CharacterClassTokenGroupValue(int tokenId, CharacterClass charClass, int minLength = 0)
            : this(tokenId, charClass, minLength, Guid.Empty, Guid.Empty) { }


        /// <summary>
        ///     create a new character class token group value
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="charClass">character class</param>
        public CharacterClassTokenGroupValue(int tokenId, CharacterClass charClass, int minLength, Guid parser, Guid minLengthMessageId) {
            TokenId = tokenId;
            MinLength = minLength;
            CharClass = charClass;
            ValueParser = parser;

            if (minLengthMessageId != Guid.Empty)
                MinLengthMessage = minLengthMessageId;
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
        public int MinLength { get; }

        /// <summary>
        ///     error message
        /// </summary>
        private Guid MinLengthMessage { get; }
            = TokenizerBase.UnexpectedEndOfToken;

        private Guid ValueParser { get; }
            = Guid.Empty;

        /// <summary>
        ///     test if a character matches the given class
        /// </summary>
        /// <param name="input">char to test</param>
        /// <returns></returns>
        private bool MatchesClass(char input)
            => CharClass.Matches(input);

        /// <summary>
        ///     parse the complete token for a character class
        /// </summary>
        public override Token Tokenize(TokenizerState state) {
            using (var parsedValue = ValueParser != Guid.Empty ? PoolFactory.FetchStringBuilder() : null) {

                if (!state.AtEof) {
                    var currentChar = state.NextChar(false);
                    while (MatchesClass(currentChar)) {
                        state.Append(currentChar);

                        if (parsedValue != null)
                            parsedValue.Data.Append(currentChar);

                        if (state.AtEof)
                            break;
                        else
                            currentChar = state.NextChar(false);
                    }

                    if (MatchesClass(currentChar)) {
                        state.Append(currentChar);
                        if (parsedValue != null)
                            parsedValue.Data.Append(currentChar);
                    }
                }

                if (MinLength > 0 && state.Length < MinLength)
                    state.Error(MinLengthMessage);

                if (ValueParser != Guid.Empty) {
                    var value = parsedValue.Data.ToString().Pool();
                    var parsed = Literals.NumberLiteral(value, ValueParser);
                    return new Token(TokenId, state.CurrentPosition, state.GetBufferContent(), parsed);
                }
                else
                    return new Token(TokenId, state);
            }
        }
    }
}