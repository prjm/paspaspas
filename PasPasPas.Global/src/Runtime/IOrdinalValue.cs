using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for ordinal values
    /// </summary>
    public interface IOrdinalValue : ITypeReference {

        /// <summary>
        ///     get the ordinal value of a given value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        ITypeReference GetOrdinalValue(ITypeRegistry types);
    }
}
