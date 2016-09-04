using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     tokenizer for compiler directives
    /// </summary>
    public class CompilerDirectiveTokenizerWithLookahead : TokenizerWithLookahead {

        /// <summary>
        ///     no macors avail
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        protected override bool IsMacroToken(PascalToken nextToken)
            => false;

        /// <summary>
        ///     test if a token is valid and relevant
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        protected override bool IsValidToken(PascalToken nextToken)
            => nextToken.Kind != PascalToken.WhiteSpace &&
            nextToken.Kind != PascalToken.ControlChar;

        /// <summary>
        ///     do nothing
        /// </summary>
        /// <param name="nextToken"></param>
        protected override void ProcssMacroToken(PascalToken nextToken) {
        }
    }
}