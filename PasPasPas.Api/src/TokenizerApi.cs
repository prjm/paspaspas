using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Parsing;
using PasPasPas.Options.Bundles;
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
        /// <param name="optionsSet">options (can be <c>null</c>)</param>
        public TokenizerApi(IParserEnvironment parserEnvironment, IOptionSet optionsSet = null) {
            Environment = parserEnvironment;
            Options = optionsSet ?? new OptionSet(Environment);
            Readers = new ReaderApi(parserEnvironment);
        }

        /// <summary>
        ///     create a buffered tokenizer with lookahead symbols
        /// </summary>
        /// <param name="data">data to read</param>
        /// <returns></returns>
        public ITokenizer CreateBufferedTokenizer(IReaderInput data) {
            var tokenizer = CreateTokenizer(data);
            return new TokenizerWithLookahead(Environment, Options, tokenizer, TokenizerMode.Standard);
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
        /// <param name="input">input</param>
        /// <returns></returns>
        public ITokenizer CreateTokenizer(IReaderInput input) {
            var fileReader = Readers.CreateReader(input);
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
