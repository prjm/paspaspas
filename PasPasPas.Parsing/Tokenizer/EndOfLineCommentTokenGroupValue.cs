﻿using System.Text;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     token group for end of line comments
    /// </summary>
    public class EndOfLineCommentTokenGroupValue : PatternContinuation {

        public override Token Tokenize(ITokenizerState state) {
            var found = false;

            while (!state.AtEof && !found) {

                var currentChar = state.NextChar(true);

                while (IsNewLineChar(currentChar) && !state.AtEof) {
                    currentChar = state.NextChar(false);
                    if (IsNewLineChar(currentChar))
                        state.Append(currentChar);
                    found = true;
                }
            }

            return new Token(TokenKind.Comment, state);
        }

    }
}

