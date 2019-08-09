using PasPasPas.Globals.Api;
using PasPasPas.Globals.Files;

namespace PasPasPas.Infrastructure.Files {

    internal class DefaultResolver : IInputResolver {

        /// <summary>
        ///     create a new resolver
        /// </summary>
        /// <param name="api"></param>
        /// <param name="resolver"></param>
        public DefaultResolver(IReaderApi api, Resolver resolver) {
            Api = api;
            Resolver = resolver;
        }


        /// <summary>
        ///     resolver function
        /// </summary>
        public Resolver Resolver { get; }

        /// <summary>
        ///     reader API
        /// </summary>
        public IReaderApi Api { get; }

        /// <summary>
        ///     resolve a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IReaderInput Resolve(FileReference file)
            => Resolver.Invoke(file, Api);
    }
}
