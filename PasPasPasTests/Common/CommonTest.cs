using PasPasPas.Api;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing;
using PasPasPas.Typings.Common;

namespace PasPasPasTests.Common {

    public class CommonTest {

        /// <summary>
        ///     get the basic environment
        /// </summary>
        /// <returns></returns>
        protected ITypedEnvironment CreateEnvironment()
            => new DefaultEnvironment();
    }
}
