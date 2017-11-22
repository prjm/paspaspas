using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Typings.Common {

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
