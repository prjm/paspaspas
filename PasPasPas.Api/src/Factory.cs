using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;

namespace PasPasPas.Api {

    /// <summary>
    ///     PasPasPas API factory
    /// </summary>
    public static class Factory {

        /// <summary>
        ///     create a global environment
        /// </summary>
        /// <returns></returns>
        public static IEnvironment CreateEnvironment()
            => new DefaultEnvironment();

        /// <summary>
        ///     create a standard reader API
        /// </summary>
        /// <param name="environment">environment</param>
        /// <returns></returns>
        public static IReaderApi CreateReaderApi(IEnvironment environment)
            => new ReaderApi(environment);

    }
}
