using System.Text;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for end of line comments
    /// </summary>
    public sealed class EndOfLineCommentTokenGroupValue : PatternContinuation {

        public override Token Tokenize(TokenizerState state) {
            var found = false;

            while (!state.AtEof && !found) {
                found = IsNewLineChar(state.LookAhead());
                if (!found)
                    state.NextChar(true);
            }

            return new Token(TokenKind.Comment, state);
        }

    }
}

