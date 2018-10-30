using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for tokenizing services
    /// </summary>
    public class TokenizerApi {

        private readonly ReaderApi reader;
        private readonly OptionSet options;
        private readonly IParserEnvironment env;

        /// <summary>
        ///     create a new tokenizer
        /// </summary>
        /// <param name="parserEnvironment">environment</param>
        /// <param name="optionsSet">options (can be <c>null</c>)</param>
        public TokenizerApi(IParserEnvironment parserEnvironment, OptionSet optionsSet = null) {
            env = parserEnvironment;
            options = optionsSet ?? new OptionSet(env);
            reader = new ReaderApi(parserEnvironment);
        }

        /// <summary>
        ///     create a tokenizer for a given path
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>tokenizer</returns>
        public ITokenizer CreateTokenizerForPath(string path) {
            var fileReader = reader.CreateReaderForPath(path);
            return CreateTokenizer(fileReader);
        }

        /// <summary>
        ///     create a buffered tokenizer with lookahead symbols
        /// </summary>
        /// <param name="path">path to read</param>
        /// <returns></returns>
        public ITokenizer CreateBufferedTokenizerForPath(string path) {
            var tokenizer = CreateTokenizerForPath(path);
            return new TokenizerWithLookahead(env, options, tokenizer, TokenizerMode.Standard);
        }

        /// <summary>
        ///     create a tokenizer
        /// </summary>
        /// <param name="fileReader"></param>
        /// <returns></returns>
        private ITokenizer CreateTokenizer(StackedFileReader fileReader)
            => new TokenizerBase(env, CreateStandardPatterns(), fileReader);

        private InputPatterns CreateStandardPatterns()
            => env.Patterns.StandardPatterns;

        /// <summary>
        ///     create a tokenizer for a string
        /// </summary>
        /// <param name="virtualPath">virtual path</param>
        /// <param name="content">string to read</param>
        /// <returns></returns>
        public ITokenizer CreateTokenizerForString(string virtualPath, string content) {
            var fileReader = reader.CreateReaderForString(virtualPath, content);
            return CreateTokenizer(fileReader);
        }

        /// <summary>
        ///     access to reader api
        /// </summary>
        public ReaderApi Readers
            => reader;

        /// <summary>
        ///     log manager
        /// </summary>
        public ILogManager Log
            => env.Log;
    }
}
