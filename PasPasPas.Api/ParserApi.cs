using PasPasPas.Infrastructure.Environment;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing;
using PasPasPas.Parsing.Parser;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for tokenizer api
    /// </summary>
    public class ParserApi {

        private readonly IParserEnvironment env;
        private readonly TokenizerApi tokenizerApi;
        private readonly OptionSet parseOptions;

        /// <summary>
        ///     create a new parser api
        /// </summary>
        /// <param name="parserEnvironment"></param>
        /// <param name="options">options</param>
        public ParserApi(IParserEnvironment parserEnvironment, OptionSet options = null) {
            env = parserEnvironment;
            parseOptions = options ?? new OptionSet(parserEnvironment);
            tokenizerApi = new TokenizerApi(parserEnvironment, options);
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
            var parser = new StandardParser(env, parseOptions, reader);
            return parser;
        }
        /// <summary>
        ///     create a parser for a given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IParser CreateParserForPath(string path) {
            var reader = tokenizerApi.Readers.CreateReaderForPath(path);
            var parser = new StandardParser(env, parseOptions, reader);
            return parser;
        }


    }
}
