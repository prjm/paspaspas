using System.Text;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for end of line comments
    /// </summary>
    public sealed class EndOfLineCommentTokenGroupValue : PatternContinuation {

        public override Token Tokenize(TokenizerState state) {
            var found = false;
            var currentChar = '\0';

            while (!state.AtEof && !found) {

                currentChar = state.NextChar(false);
                found = IsNewLineChar(currentChar);

                if (!found)
                    state.Append(currentChar);
            }

            if (IsNewLineChar(currentChar))
                state.PreviousChar();

            return new Token(TokenKind.Comment, state);
        }

    }
}

