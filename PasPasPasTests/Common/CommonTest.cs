using PasPasPas.Api;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing;

namespace PasPasPasTests.Common {

    public class CommonTest {

        /// <summary>
        ///     get the basic environment
        /// </summary>
        /// <returns></returns>
        protected IParserEnvironment CreateEnvironment()
            => new DefaultEnvironment();
    }
}
