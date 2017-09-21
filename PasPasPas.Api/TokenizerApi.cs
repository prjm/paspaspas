using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for tokenizing services
    /// </summary>
    public class TokenizerApi {

        private readonly IFileAccess standardFileAccess;
        private readonly ReaderApi reader;
        private readonly TokenizerApiOptions options;
        private readonly ILogManager log;

        /// <summary>
        ///     create a new tokenizer ap
        /// </summary>
        /// <param name="access">file access</param>
        /// <param name="options">options (not required)</param>
        public TokenizerApi(IFileAccess access, TokenizerApiOptions options = null) {
            standardFileAccess = access;
            reader = new ReaderApi(access);
            log = new LogManager();

            if (options != null)
                this.options = options;
            else
                this.options = new TokenizerApiOptions();

            RegisterStatics();
        }

        private void RegisterStatics() {
            StaticEnvironment.Register(Literals.ParsedIntegers, () => new IntegerParser(false));
            StaticEnvironment.Register(Literals.ParsedHexNumbers, () => new IntegerParser(true));
            StaticEnvironment.Register(Literals.ConvertedCharLiterals, () => new CharLiteralConverter());
            StaticEnvironment.Register(Literals.ConvertedRealLiterals, () => new RealLiteralConverter());
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
        ///     create a tokenizer
        /// </summary>
        /// <param name="fileReader"></param>
        /// <returns></returns>
        private ITokenizer CreateTokenizer(StackedFileReader fileReader)
            => new TokenizerBase(log, new StandardPatterns(), fileReader);

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
        ///     log manager
        /// </summary>
        public ILogManager Log
            => log;
    }
}
