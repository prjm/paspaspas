using PasPasPas.Global.Runtime;

namespace PasPasPas.Global.Types {

    /// <summary>
    ///     symbol by reference
    /// </summary>
    public interface IRefSymbol {

        /// <summary>
        ///     get the type of this symbol
        /// </summary>
        int TypeId { get; }

    }
}
