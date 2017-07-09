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

        private IFileAccess standardFileAccess;

        private ReaderApi reader;

        /// <summary>
        ///     create a new tokenizer ap
        /// </summary>
        /// <param name="access">file access</param>
        public TokenizerApi(IFileAccess access) {
            standardFileAccess = access;
            reader = new ReaderApi(access);
        }

        /// <summary>
        ///     create a tokenizer for a given path
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>tokenizer</returns>
        public ITokenizer CreateTokenizerForPath(string path) {
            var log = new LogManager();
            var fileReader = reader.CreateReaderForPath(path);
            var tokenizer = new StandardTokenizer(log, fileReader);
            return tokenizer;
        }
    }
}
