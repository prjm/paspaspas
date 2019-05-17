using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {
    /// <summary>
    ///     token group value based on a sequence
    /// </summary>
    public class SequenceGroupTokenValue : PatternContinuation {

        /// <summary>
        ///     <c>true</c> if the parsed sequence should be stored
        /// </summary>
        public bool StoreValue { get; private set; }

        /// <summary>
        ///     token id
        /// </summary>
        public int TokenId { get; private set; }

        /// <summary>
        ///     end sequence
        /// </summary>
        public string EndSequence { get; private set; }

        /// <summary>
        ///     Create a new sequence token group value
        /// </summary>
        /// <param name="tokenId">parsed token in</param>
        /// <param name="endSequence">end sequence</param>
        /// <param name="storeValue">if <c>true</c> the stored value is passed to the token</param>
        public SequenceGroupTokenValue(int tokenId, string endSequence, bool storeValue = false) {
            TokenId = tokenId;
            EndSequence = endSequence;
            StoreValue = storeValue;
        }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(TokenizerState state) {
            var found = false;


            using (var builder = StoreValue ? state.FetchStringBuilder() : null) {

                while (!found && !state.AtEof) {
                    if (StoreValue)
                        builder.Item.Append(state.CurrentCharacter);

                    state.NextChar(true);
                    found = state.BufferEndsWith(EndSequence);
                }

                if (!found)
                    found = state.BufferEndsWith(EndSequence);

                if (!found)
                    state.Error(TokenizerBase.UnexpectedEndOfToken);

                if (StoreValue) {
                    builder.Item.Remove(0, 1);
                    for (var i = 0; i < EndSequence.Length - 1 && builder.Item.Length > 0; i++)
                        builder.Item.Remove(builder.Item.Length - 1, 1);
                    var stringValue = state.RuntimeValues.Strings.ToUnicodeString(state.Environment.StringPool.PoolString(builder.Item));
                    return new Token(TokenId, state, stringValue);
                }
                else
                    return new Token(TokenId, state);
            }
        }
    }
}