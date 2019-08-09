using PasPasPas.Globals.Api;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Parsing.Parser;

namespace PasPasPas.Api {

    /// <summary>
    ///     utility functions
    /// </summary>
    public static class CommonApi {

        /// <summary>
        ///     create a resolver for local files
        /// </summary>
        /// <param name="readerApi"></param>
        /// <returns></returns>
        public static IInputResolver CreateAnyFileResolver(IReaderApi readerApi) {
            IReaderInput doResolve(FileReference file, IReaderApi api)
                => api.CreateInputForPath(file);

            return readerApi.CreateInputResolver(doResolve);
        }

        /// <summary>
        ///     create a stacked file reader
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IStackedFileReader CreateReaderForFiles(string file) {
            var env = Factory.CreateEnvironment();
            var api = Factory.CreateReaderApi(env);
            var fileRef = api.CreateFileRef(file);
            var data = CreateAnyFileResolver(api);
            return api.CreateReader(data, fileRef);
        }

        /// <summary>
        ///     create a tokenizer for a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static ITokenizer CreateTokenizerForFiles(string file) {
            var env = Factory.CreateEnvironment();
            var api = Factory.CreateTokenizerApi(env, default);
            var fileRef = api.Readers.CreateFileRef(file);
            var data = CreateAnyFileResolver(api.Readers);
            return api.CreateTokenizer(data, fileRef);
        }

        /// <summary>
        ///     create a buffered tokenizer for a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static ITokenizer CreateBufferedTokenizer(string file) {
            var env = Factory.CreateEnvironment();
            var api = Factory.CreateTokenizerApi(env, default);
            var fileRef = api.Readers.CreateFileRef(file);
            var data = CreateAnyFileResolver(api.Readers);
            return api.CreateBufferedTokenizer(data, fileRef);
        }



        /// <summary>
        ///     create a standard parser
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IParser CreateParserForFiles(string file) {
            var env = Factory.CreateEnvironment();
            var api = Factory.CreateParserApi(env, default);
            var fileRef = api.Tokenizer.Readers.CreateFileRef(file);
            var data = CreateAnyFileResolver(api.Tokenizer.Readers);
            return api.CreateParser(data, fileRef);
        }

        /// <summary>
        ///     create a resolver for a single string
        /// </summary>
        /// <param name="api"></param>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IInputResolver CreateResolverForSingleString(IReaderApi api, FileReference path, string content) {
            IReaderInput doResolve(FileReference file, IReaderApi aapi) {
                if (path.Equals(file))
                    return aapi.CreateInputForString(file, content);

                return default;
            }
            return api.CreateInputResolver(doResolve);
        }
    }
}
