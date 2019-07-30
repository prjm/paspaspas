using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Globals.Api {

    /// <summary>
    ///     interface for tokenizer service
    /// </summary>
    public interface ITokenizerApi {

        /// <summary>
        ///     nested reader API
        /// </summary>
        IReaderApi Readers { get; }

        /// <summary>
        ///     create a new tokenizer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ITokenizer CreateTokenizer(IReaderInput data);

        /// <summary>
        ///     access the global environment
        /// </summary>
        IParserEnvironment Environment { get; }

        /// <summary>
        ///     create a buffered tokenizer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ITokenizer CreateBufferedTokenizer(IReaderInput data);
    }
}
