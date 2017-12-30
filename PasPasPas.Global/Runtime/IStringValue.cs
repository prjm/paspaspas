using PasPasPas.Global.Runtime;

namespace PasPasPas.Infrastructure.Common {

    /// <summary>
    ///     string value
    /// </summary>
    public interface IStringValue : IValue {

        /// <summary>
        ///     get string valu
        /// </summary>
        string AsUnicodeString { get; }

    }
}
