using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Parser;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for tokenizer api
    /// </summary>
    public class ParserApi {


        private readonly IFileAccess standardFileAccess;
        private readonly TokenizerApi tokenizerApi;
        private readonly OptionSet parseOptions;
        /// <summary>
        ///     create a new parser api
        /// </summary>
        /// <param name="access">file access</param>
        /// <param name="options">parser options</param>
        public ParserApi(IFileAccess access, ParserApiOptions options = null) {
            var tokenizerOptions = new TokenizerApiOptions() {
                Log = options?.Log
            };

            standardFileAccess = access;
            parseOptions = options?.Options ?? new OptionSet(standardFileAccess);
            tokenizerApi = new TokenizerApi(access, tokenizerOptions);
            RegisterStatics();
        }

        private void RegisterStatics() {
            //..
        }

        /// <summary>
        ///     create a parser for a given input string
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="input">input</param>
        /// <returns></returns>
        public IParser CreateParserForString(string path, string input) {
            var reader = tokenizerApi.Readers.CreateReaderForString(path, input);
            var parser = new StandardParser(tokenizerApi.Log, parseOptions, reader);
            return parser;
        }
        /// <summary>
        ///     create a parser for a given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IParser CreateParserForPath(string path) {
            var reader = tokenizerApi.Readers.CreateReaderForPath(path);
            var parser = new StandardParser(tokenizerApi.Log, parseOptions, reader);
            return parser;
        }


    }
}
