using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for tokenizing services
    /// </summary>
    public class TokenizerApi {

        private readonly ReaderApi reader;
        private readonly StaticEnvironment environment;
        private readonly OptionSet options;

        /// <summary>
        ///     create a new tokenizer ap
        /// </summary>
        /// <param name="staticEnvironment">static environment</param>
        /// <param name="optionsSet">options (can be <c>null</c>)</param>
        public TokenizerApi(StaticEnvironment staticEnvironment, OptionSet optionsSet = null) {
            environment = staticEnvironment;
            options = optionsSet ?? new OptionSet(environment);
            reader = new ReaderApi(staticEnvironment);
            RegisterStatics(staticEnvironment);
        }

        private void RegisterStatics(StaticEnvironment environment) {
            environment.Register(StaticDependency.ParsedIntegers, new IntegerParser(false));
            environment.Register(StaticDependency.ParsedHexNumbers, new IntegerParser(true));
            environment.Register(StaticDependency.ConvertedCharLiterals, new CharLiteralConverter());
            environment.Register(StaticDependency.ConvertedRealLiterals, new RealLiteralConverter());
            environment.Register(StaticDependency.TokenSequencePool, new ObjectPool<TokenizerWithLookahead.TokenSequence>());
            environment.Register(StaticDependency.TokenizerPatternFactory, new PatternFactory());
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
            return new TokenizerWithLookahead(environment, options, tokenizer, TokenizerMode.Standard);
        }

        /// <summary>
        ///     create a tokenizer
        /// </summary>
        /// <param name="fileReader"></param>
        /// <returns></returns>
        private ITokenizer CreateTokenizer(StackedFileReader fileReader)
            => new Tokenizer(environment, CreateStandardPatterns(), fileReader);

        private InputPatterns CreateStandardPatterns()
            => environment.Require<PatternFactory>(StaticDependency.TokenizerPatternFactory).StandardPatterns;

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
            => environment.Require<ILogManager>(StaticDependency.LogManager);
    }
}
