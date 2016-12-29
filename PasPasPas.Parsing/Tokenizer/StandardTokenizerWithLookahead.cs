using PasPasPas.Infrastructure.Input;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     pascal tokenizer with lookahead
    /// </summary>
    public class StandardTokenizerWithLookahead : TokenizerWithLookahead {


        private readonly ParserServices environment;

        /// <summary>
        ///     create a new pascal tokenizer
        /// </summary>
        /// <param name="environment"></param>
        public StandardTokenizerWithLookahead(ParserServices environment) {
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
        protected override bool IsMacroToken(Token nextToken)
            => nextToken.Kind == TokenKind.Preprocessor;

        /// <summary>
        ///     test if a token is a valid token
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        protected override bool IsValidToken(Token nextToken)
            => (nextToken.Kind != TokenKind.WhiteSpace) &&
            nextToken.Kind != TokenKind.ControlChar &&
            nextToken.Kind != TokenKind.Comment &&
            nextToken.Kind != TokenKind.Preprocessor &&
            (!environment.Options.ConditionalCompilation.Skip);

        /// <summary>
        ///     process preprocessor token
        /// </summary>
        /// <param name="nextToken"></param>
        protected override void ProcessMacroToken(Token nextToken) {
            using (var input = new StringInput(CompilerDirectiveTokenizer.Unwrap(nextToken.Value), nextToken.FilePath))
            using (var reader = new StackedFileReader()) {
                var parser = new CompilerDirectiveParser(environment, reader);
                var tokenizer = new CompilerDirectiveTokenizer(environment, reader);
                reader.AddFile(input);
                parser.BaseTokenizer = tokenizer;
                ISyntaxPart result = parser.Parse();
                CompilerDirectiveVisitor visitor = new CompilerDirectiveVisitor();
                CompilerDirectiveVisitorOptions visitorOptions = new CompilerDirectiveVisitorOptions();
                visitorOptions.Environment = environment;
                VisitorHelper.AcceptVisitor(result, visitor, visitorOptions);
            }
        }


    }
}
