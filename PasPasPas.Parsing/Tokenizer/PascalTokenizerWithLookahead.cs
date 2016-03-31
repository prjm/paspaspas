using PasPasPas.Api;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Service;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Parser;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     pascal tokenizer with lookahead
    /// </summary>
    public class PascalTokenizerWithLookahead : TokenizerWithLookahead {


        private readonly ServiceProvider environment;

        /// <summary>
        ///     create a new pascal tokenizer
        /// </summary>
        /// <param name="environment"></param>
        public PascalTokenizerWithLookahead(ServiceProvider environment) {
            this.environment = environment;
        }

        /// <summary>
        ///     compiler options
        /// </summary>
        public IOptionSet Options
            => environment.Resolve(StandardServices.CompilerConfigurationServiceClass) as IOptionSet;

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
            var parser = new CompilerDirectiveParser(environment);
            var tokenizer = new CompilerDirectiveTokenizer();
            using (var input = new StringInput(CompilerDirectiveTokenizer.Unwrap(nextToken.Value)))
            using (var reader = new StackedFileReader()) {
                reader.AddFile(input);
                parser.BaseTokenizer = tokenizer;
                parser.ParseCompilerDirective();
            }
        }


    }
}
