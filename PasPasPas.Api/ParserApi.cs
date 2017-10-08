using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Parser;

namespace PasPasPas.Api {

    /// <summary>
    ///     encapsulation for tokenizer api
    /// </summary>
    public class ParserApi {


        private readonly StaticEnvironment environment;
        private readonly TokenizerApi tokenizerApi;
        private readonly OptionSet parseOptions;
        /// <summary>
        ///     create a new parser api
        /// </summary>
        /// <param name="staticEnvironment">Static environment</param>
        /// <param name="options">options</param>
        public ParserApi(StaticEnvironment staticEnvironment, OptionSet options = null) {
            environment = staticEnvironment;
            parseOptions = options ?? new OptionSet(environment);
            tokenizerApi = new TokenizerApi(environment, options);
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
            var parser = new StandardParser(environment, parseOptions, reader);
            return parser;
        }
        /// <summary>
        ///     create a parser for a given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IParser CreateParserForPath(string path) {
            var reader = tokenizerApi.Readers.CreateReaderForPath(path);
            var parser = new StandardParser(environment, parseOptions, reader);
            return parser;
        }


    }
}
