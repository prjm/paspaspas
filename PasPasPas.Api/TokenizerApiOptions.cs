using PasPasPas.Infrastructure.Log;

namespace PasPasPas.Api {

    /// <summary>
    ///     options for tokenizers
    /// </summary>
    public class TokenizerApiOptions {

        /// <summary>
        ///     if <c>true</c>, whitespace literal tokens are kept
        /// </summary>
        /// <remarks>default <c>false</c></remarks>
        public bool KeepWhitespace { get; set; }
            = false;

        /// <summary>
        ///     log manager
        /// </summary>
        public ILogManager Log { get; set; }
    }
}