using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Options;

namespace PasPasPas.Api {

    /// <summary>
    ///     PasPasPas API factory
    /// </summary>
    public static class Factory {

        /// <summary>
        ///     create a global environment
        /// </summary>
        /// <returns></returns>
        public static IAssemblyBuilderEnvironment CreateEnvironment()
            => new DefaultEnvironment();

        /// <summary>
        ///     create a standard reader API
        /// </summary>
        /// <param name="environment">environment</param>
        /// <returns></returns>
        public static IReaderApi CreateReaderApi(IEnvironment environment)
            => new ReaderApi(environment);

        /// <summary>
        ///     create a new tokenizer API
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ITokenizerApi CreateTokenizerApi(IParserEnvironment environment, IOptionSet options)
            => new TokenizerApi(environment, options);

        /// <summary>
        ///     create a new parser API
        /// </summary>
        /// <param name="env"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IParserApi CreateParserApi(ITypedEnvironment env, IOptionSet options)
            => new ParserApi(env, options);

    }
}
