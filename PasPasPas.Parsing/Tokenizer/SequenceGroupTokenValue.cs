using System.Text;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Infrastructure.Log;

namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     token group value based on a sequence
    /// </summary>
    public abstract class SequenceGroupTokenValue : PatternContinuation {

        /// <summary>
        ///     token id
        /// </summary>
        protected abstract int TokenId { get; }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(StringBuilder buffer, int position, ITokenizer tokenizer) {
            var found = false;

            while ((!found) && (!tokenizer.AtEof)) {
                found = buffer.EndsWith(EndSequence);

                if (!found)
                    FetchAndAppendChar(buffer, tokenizer);
            }

            found = buffer.EndsWith(EndSequence);

            if (!found)
                tokenizer.Log.ProcessMessage(new LogMessage(MessageSeverity.Error, TokenizerBase.TokenizerLogMessage, TokenizerBase.UnexpectedEndOfToken, EndSequence));

            return new Token(TokenId, position, buffer.ToString());
        }

        /// <summary>
        ///     end sequence
        /// </summary>
        protected abstract string EndSequence { get; }
    }
}