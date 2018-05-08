using System;
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
        /// <param name="minLength">minimun token length</param>
        public CharacterClassTokenGroupValue(int tokenId, CharacterClass charClass, int minLength = 0)
            : this(tokenId, charClass, minLength, LiteralParserKind.Undefined, Guid.Empty) { }

        /// <summary>
        ///     create a new character class token group value
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="charClass">character class</param>
        /// <param name="literalParserKind">literal parser (optional)</param>
        /// <param name="minLength">minimal length</param>
        /// <param name="minLengthMessageId">error message id for too short tokens</param>
        public CharacterClassTokenGroupValue(int tokenId, CharacterClass charClass, int minLength, LiteralParserKind literalParserKind, Guid minLengthMessageId) {
            TokenId = tokenId;
            MinLength = minLength;
            CharClass = charClass;
            ValueParser = literalParserKind;

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
            = Tokenizer.UnexpectedEndOfToken;

        private LiteralParserKind ValueParser { get; }
            = LiteralParserKind.Undefined;

        /// <summary>
        ///     parse the complete token for a character class
        /// </summary>
        public override Token Tokenize(TokenizerState state) {
            var parseValue = ValueParser != LiteralParserKind.Undefined;

            using (var parsedValue = parseValue ? state.FetchStringBuilder() : null) {

                if (!state.AtEof) {
                    while (CharClass.Matches(state.LookAhead())) {
                        var currentChar = state.NextChar(true);
                        parsedValue?.Data.Append(currentChar);

                        if (state.AtEof)
                            break;
                    }
                }

                if (MinLength > 0 && state.Length < MinLength)
                    state.Error(MinLengthMessage);

                if (parseValue) {
                    var value = state.Environment.StringPool.PoolString(parsedValue.Data.ToString());
                    var parsed = state.ParserLiteral(value, ValueParser);
                    return new Token(TokenId, state, parsed);
                }
                else
                    return new Token(TokenId, state);
            }
        }
    }
}