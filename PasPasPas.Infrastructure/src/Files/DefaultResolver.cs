using PasPasPas.Globals.Api;
using PasPasPas.Globals.Files;

namespace PasPasPas.Infrastructure.Files {

    internal class DefaultResolver : IInputResolver {

        /// <summary>
        ///     create a new resolver
        /// </summary>
        /// <param name="api"></param>
        /// <param name="resolver"></param>
        public DefaultResolver(Resolver resolver, Checker checker) {
            Resolver = resolver;
            Checker = checker;
        }

        /// <summary>
        ///     resolver function
        /// </summary>
        public Resolver Resolver { get; }

        /// <summary>
        ///     checker function
        /// </summary>
        public Checker Checker { get; }

        /// <summary>
        ///     try to resolve a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool CanResolve(FileReference file)
            => Checker.Invoke(file);

        /// <summary>
        ///     resolve a file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="api"></param>
        /// <returns></returns>
        public IReaderInput Resolve(IReaderApi api, FileReference file)
            => Resolver.Invoke(file, api);
    }
}
