using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Options.Bundles;

namespace PasPasPas.Api {

    /// <summary>
    ///     PasPasPas API factory
    /// </summary>
    public static class Factory {

        /// <summary>
        ///     create a global environment
        /// </summary>
        /// <returns></returns>
        public static IAssemblyBuilderEnvironment CreateEnvironment(NativeIntSize intSize = NativeIntSize.Undefined)
            => new DefaultEnvironment(intSize);

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
        /// <param name="options"></param>
        /// <returns></returns>
        public static ITokenizerApi CreateTokenizerApi(IOptionSet options)
            => new TokenizerApi(options);

        /// <summary>
        ///     create a new option set
        /// </summary>
        /// <returns></returns>
        public static IOptionSet CreateOptions(IInputResolver resolver, IEnvironment environment)
            => new OptionSet(resolver, environment);

        /// <summary>
        ///     create a new option set
        /// </summary>
        /// <returns></returns>
        public static IOptionSet CreateOptions(IOptionSet options)
            => new OptionSet(options);


        /// <summary>
        ///     create a new parser API
        /// </summary>
        /// <param name="env"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IParserApi CreateParserApi(IOptionSet options)
            => new ParserApi(options);

        /// <summary>
        ///     create an input resolver
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="checker"></param>
        /// <returns></returns>
        public static IInputResolver CreateInputResolver(Resolver resolver, Checker checker)
            => new DefaultResolver(resolver, checker);


    }
}
