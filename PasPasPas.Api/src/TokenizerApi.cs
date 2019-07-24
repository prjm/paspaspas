using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for the tokenizer
    /// </summary>
    public class TokenizerApi {

        /// <summary>
        ///     create a new tokenizer
        /// </summary>
        /// <param name="parserEnvironment">environment</param>
        /// <param name="optionsSet">options (can be <c>null</c>)</param>
        public TokenizerApi(IParserEnvironment parserEnvironment, OptionSet optionsSet = null) {
            SystemEnvironment = parserEnvironment;
            Options = optionsSet ?? new OptionSet(SystemEnvironment);
            Readers = new ReaderApi(parserEnvironment);
        }

        /// <summary>
        ///     create a buffered tokenizer with lookahead symbols
        /// </summary>
        /// <param name="data">data to read</param>
        /// <returns></returns>
        public ITokenizer CreateBufferedTokenizer(IReaderInput data) {
            var tokenizer = CreateTokenizer(data);
            return new TokenizerWithLookahead(SystemEnvironment, Options, tokenizer, TokenizerMode.Standard);
        }

        /// <summary>
        ///     create a tokenizer
        /// </summary>
        /// <param name="fileReader"></param>
        /// <returns></returns>
        private ITokenizer CreateTokenizer(IStackedFileReader fileReader)
            => new TokenizerBase(SystemEnvironment, CreateStandardPatterns(), fileReader);

        private InputPatterns CreateStandardPatterns()
            => ((PatternFactory)SystemEnvironment.Patterns).StandardPatterns;

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
            => SystemEnvironment.Log;

        /// <summary>
        ///     environment
        /// </summary>
        public IParserEnvironment SystemEnvironment { get; }

        /// <summary>
        ///     tokenizer options
        /// </summary>
        public OptionSet Options { get; }
    }
}
