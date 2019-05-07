using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;
using PasPasPas.Infrastructure.Files;
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
        ///     create a tokenizer for a given path
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>tokenizer</returns>
        public ITokenizer CreateTokenizerForPath(string path) {
            var fileReader = ReaderApi.CreateReaderForPath(path);
            return CreateTokenizer(fileReader);
        }

        /// <summary>
        ///     create a buffered tokenizer with lookahead symbols
        /// </summary>
        /// <param name="path">path to read</param>
        /// <returns></returns>
        public ITokenizer CreateBufferedTokenizerForPath(string path) {
            var tokenizer = CreateTokenizerForPath(path);
            return new TokenizerWithLookahead(SystemEnvironment, Options, tokenizer, TokenizerMode.Standard);
        }

        /// <summary>
        ///     create a tokenizer
        /// </summary>
        /// <param name="fileReader"></param>
        /// <returns></returns>
        private ITokenizer CreateTokenizer(StackedFileReader fileReader)
            => new TokenizerBase(SystemEnvironment, CreateStandardPatterns(), fileReader);

        private InputPatterns CreateStandardPatterns()
            => ((PatternFactory)SystemEnvironment.Patterns).StandardPatterns;

        /// <summary>
        ///     create a tokenizer for a string
        /// </summary>
        /// <param name="virtualPath">virtual path</param>
        /// <param name="content">string to read</param>
        /// <returns></returns>
        public ITokenizer CreateTokenizerForString(string virtualPath, string content) {
            var fileReader = ReaderApi.CreateReaderForString(virtualPath, content);
            return CreateTokenizer(fileReader);
        }

        /// <summary>
        ///     access to reader api
        /// </summary>
        public ReaderApi Readers { get; }

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
