using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;

namespace PasPasPas.Api {

    /// <summary>
    ///     parser api options
    /// </summary>
    public class ParserApiOptions {

        /// <summary>
        ///     log manager to use
        /// </summary>
        public ILogManager Log { get; set; }

        /// <summary>
        ///     parser options
        /// </summary>
        public OptionSet Options { get; internal set; }
    }
}