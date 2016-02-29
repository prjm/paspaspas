using PasPasPas.Api;
using PasPasPas.Api.Options;
using PasPasPas.Internal.Input;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     pascal tokenizer with lookahead
    /// </summary>
    public class PascalTokenizerWithLookahead : TokenizerWithLookahead {

        /// <summary>
        ///     compiler options
        /// </summary>
        public OptionSet Options { get; set; }
            = new OptionSet();

        /// <summary>
        ///     test if a token is a macro token
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        protected override bool IsMacroToken(PascalToken nextToken)
            => nextToken.Kind == PascalToken.Preprocessor;

        /// <summary>
        ///     test if a token is a valid token
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        protected override bool IsValidToken(PascalToken nextToken)
            => nextToken.Kind != PascalToken.WhiteSpace &&
            nextToken.Kind != PascalToken.ControlChar &&
            nextToken.Kind != PascalToken.Comment &&
            nextToken.Kind != PascalToken.Preprocessor;

        /// <summary>
        ///     process preprocessor token
        /// </summary>
        /// <param name="nextToken"></param>
        protected override void ProcssMacroToken(PascalToken nextToken) {
            var parser = new CompilerDirectiveParser();
            var tokenizer = new CompilerDirectiveTokenizer();
            var input = new StringInput(CompilerDirectiveTokenizer.Unwrap(nextToken.Value));
            tokenizer.Input = input;
            parser.BaseTokenizer = tokenizer;
            parser.Options = Options;
            parser.ParseCompilerDirective();
        }


    }
}
