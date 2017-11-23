using PasPasPas.Infrastructure.Utils;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     base class for structured types
    /// </summary>
    public abstract class StructuredTypeBase : TypeBase {

        /// <summary>
        ///     create a new structured type
        /// </summary>
        /// <param name="withId"></param>
        public StructuredTypeBase(int withId) : base(withId) {
        }
    }
}
