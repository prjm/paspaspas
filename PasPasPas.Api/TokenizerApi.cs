using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.Tokenizer;

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
        public TokenizerApi(IFileAccess access) {
            standardFileAccess = access;
            reader = new ReaderApi(access);
            options = new TokenizerApiOptions();
            log = new LogManager();
        }

        /// <summary>
        ///     create a tokenizer for a given path
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>tokenizer</returns>
        public ITokenizer CreateTokenizerForPath(string path) {
            var fileReader = reader.CreateReaderForPath(path);
            var tokenizer = new StandardTokenizer(log, fileReader);
            tokenizer.KeepWhitspace = options.KeepWhitespace;
            return tokenizer;
        }

        /// <summary>
        ///     create a tokenizer for a string
        /// </summary>
        /// <param name="virtualPath">virtual path</param>
        /// <param name="content">string to read</param>
        /// <returns></returns>
        public ITokenizer CreateTokenizerForString(string virtualPath, string content) {
            var fileReader = reader.CreateReaderForString(virtualPath, content);
            var tokenizer = new StandardTokenizer(log, fileReader);
            tokenizer.KeepWhitspace = options.KeepWhitespace;
            return tokenizer;
        }

        /// <summary>
        ///     log manager
        /// </summary>
        public ILogManager Log
            => log;
    }
}
