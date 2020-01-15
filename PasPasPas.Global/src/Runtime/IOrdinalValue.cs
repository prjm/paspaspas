using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for ordinal values
    /// </summary>
    public interface IOrdinalValue : IOldTypeReference {

        /// <summary>
        ///     get the ordinal value of a given value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IOldTypeReference GetOrdinalValue(ITypeRegistry types);
    }
}
