using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     token group which moves the input to eof
    /// </summary>
    public class SoftEofTokenValue : PatternContinuation {

        public override Token Tokenize(TokenizerState state) {

            while (!state.AtEof)
                state.NextChar(true);

            return new Token(TokenKind.Eof, state);
        }
    }

}
