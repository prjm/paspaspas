using PasPasPas.Infrastructure.Utils;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     boolean type
    /// </summary>
    public class BooleanType : TypeBase {

        /// <summary>
        ///     create a new boolean type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="name">name (optional)</param>
        public BooleanType(int withId, ScopedName name = null) : base(withId, name) {
        }
    }
}
