using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group which moves the input to eof
    /// </summary>
    public class SoftEofTokenValue : PatternContinuation {

        /// <summary>
        ///     create a token
        /// </summary>
        /// <param name="state">tokenizer state</param>
        /// <returns></returns>
        public override Token Tokenize(TokenizerState state) {

            while (!state.AtEof)
                state.NextChar(true);

            return new Token(TokenKind.WhiteSpace, state);
        }
    }

}
