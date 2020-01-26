using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for ordinal values
    /// </summary>
    public interface IOrdinalValue : IValue {

        /// <summary>
        ///     get the ordinal value of a given value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IValue GetOrdinalValue(ITypeRegistry types);
    }
}
