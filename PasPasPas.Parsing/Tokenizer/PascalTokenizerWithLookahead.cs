using PasPasPas.Infrastructure.Input;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     pascal tokenizer with lookahead
    /// </summary>
    public class PascalTokenizerWithLookahead : TokenizerWithLookahead {


        private readonly ParserServices environment;

        /// <summary>
        ///     create a new pascal tokenizer
        /// </summary>
        /// <param name="environment"></param>
        public PascalTokenizerWithLookahead(ParserServices environment) {
            this.environment = environment;
        }

        /// <summary>
        ///     compiler options
        /// </summary>
        public IOptionSet Options
            => environment.Options;

        /// <summary>
        ///     test if a token is a macro token
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        protected override bool IsMacroToken(PascalToken nextToken)
            => nextToken.Kind == TokenKind.Preprocessor;

        /// <summary>
        ///     test if a token is a valid token
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        protected override bool IsValidToken(PascalToken nextToken)
            => nextToken.Kind != PascalToken.WhiteSpace &&
            nextToken.Kind != PascalToken.ControlChar &&
            nextToken.Kind != TokenKind.Comment &&
            nextToken.Kind != TokenKind.Preprocessor;

        /// <summary>
        ///     process preprocessor token
        /// </summary>
        /// <param name="nextToken"></param>
        protected override void ProcssMacroToken(PascalToken nextToken) {
            using (var input = new StringInput(CompilerDirectiveTokenizer.Unwrap(nextToken.Value), nextToken.FilePath))
            using (var reader = new StackedFileReader()) {
                var parser = new CompilerDirectiveParser(environment, reader);
                var tokenizer = new CompilerDirectiveTokenizer(environment, reader);
                reader.AddFile(input);
                parser.BaseTokenizer = tokenizer;
                parser.Parse();
            }
        }


    }
}
