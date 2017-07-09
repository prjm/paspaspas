using System;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     parser services
    /// </summary>
    [Obsolete]
    public class ParserServices {

        /// <summary>
        ///     logging
        /// </summary>
        public ILogManager Log { get; }

        /// <summary>
        ///     parser options
        /// </summary>
        public IOptionSet Options { get; set; }

        /// <summary>
        ///     create a new service class for a parser
        /// </summary>
        /// <param name="log">logging manager</param>
        public ParserServices(ILogManager log) {
            Log = log;
        }

    }
}
