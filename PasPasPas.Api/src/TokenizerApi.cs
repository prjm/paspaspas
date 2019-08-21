using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for the tokenizer
    /// </summary>
    internal class TokenizerApi : ITokenizerApi {

        /// <summary>
        ///     create a new tokenizer
        /// </summary>
        /// <param name="parserEnvironment">environment</param>
        /// <param name="resolver"></param>
        public TokenizerApi(IInputResolver resolver, IParserEnvironment parserEnvironment)
             : this(Factory.CreateOptions(resolver, parserEnvironment)) { }


        public TokenizerApi(IOptionSet options) {
            Environment = options.Environment as IParserEnvironment;
            Options = options;
            Readers = new ReaderApi(Options.Environment);
        }

        /// <summary>
        ///     create a buffered tokenizer with lookahead symbols
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public ITokenizer CreateBufferedTokenizer(IInputResolver resolver, IFileReference path) {
            var tokenizer = CreateTokenizer(resolver, path);
            return new TokenizerWithLookahead(this, tokenizer, TokenizerMode.Standard);
        }

        /// <summary>
        ///     create a tokenizer
        /// </summary>
        /// <param name="fileReader"></param>
        /// <returns></returns>
        private ITokenizer CreateTokenizer(IStackedFileReader fileReader)
            => new TokenizerBase(Environment, CreateStandardPatterns(), fileReader);

        private InputPatterns CreateStandardPatterns()
            => ((PatternFactory)Environment.Patterns).StandardPatterns;

        /// <summary>
        ///     create a tokenizer for a string
        /// </summary>
        /// <param name="path"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public ITokenizer CreateTokenizer(IInputResolver resolver, IFileReference path) {
            var fileReader = Readers.CreateReader(resolver, path);
            return CreateTokenizer(fileReader);
        }

        /// <summary>
        ///     access to reader API
        /// </summary>
        public IReaderApi Readers { get; }

        /// <summary>
        ///     log manager
        /// </summary>
        public ILogManager Log
            => Environment.Log;

        /// <summary>
        ///     environment
        /// </summary>
        public IParserEnvironment Environment { get; }

        /// <summary>
        ///     tokenizer options
        /// </summary>
        public IOptionSet Options { get; }
    }
}
