using PasPasPas.Infrastructure.Service;
using PasPasPas.Options.Bundles;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     parser services
    /// </summary>
    public class ParserServices {

        /// <summary>
        ///     standard environment
        /// </summary>
        public StandardServices Environment { get; }

        /// <summary>
        ///     parser options
        /// </summary>
        public IOptionSet Options { get; set; }

        /// <summary>
        ///     create a new service class for a parser
        /// </summary>
        /// <param name="environment">standard services</param>
        public ParserServices(StandardServices environment) {
            Environment = environment;
        }

    }
}
