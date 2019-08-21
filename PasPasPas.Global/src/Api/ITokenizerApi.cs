using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;
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
        /// <param name="path"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        ITokenizer CreateTokenizer(IInputResolver resolver, IFileReference path);

        /// <summary>
        ///     access the global environment
        /// </summary>
        IParserEnvironment Environment { get; }

        /// <summary>
        ///     option set
        /// </summary>
        IOptionSet Options { get; }

        /// <summary>
        ///     create a buffered tokenizer
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        ITokenizer CreateBufferedTokenizer(IInputResolver resolver, IFileReference path);
    }
}
