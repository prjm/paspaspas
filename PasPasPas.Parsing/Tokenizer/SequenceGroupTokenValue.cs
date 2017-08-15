using System.Text;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Infrastructure.Log;

namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     token group value based on a sequence
    /// </summary>
    public class SequenceGroupTokenValue : PatternContinuation {

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
        /// <param name="tokenId"></param>
        /// <param name="endSequence"></param>
        public SequenceGroupTokenValue(int tokenId, string endSequence) {
            TokenId = tokenId;
            EndSequence = endSequence;
        }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(ITokenizerState state) {
            var found = false;

            while ((!found) && (!state.AtEof)) {
                state.NextChar(true);
                found = state.BufferEndsWith(EndSequence);
            }

            if (!found)
                found = state.BufferEndsWith(EndSequence);

            if (!found)
                state.Error(TokenizerBase.UnexpectedEndOfToken);

            return new Token(TokenId, state);
        }

    }
}