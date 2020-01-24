using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     value of an enumeration
    /// </summary>
    public interface IEnumeratedValue : ITypeSymbol {

        /// <summary>
        ///     constant value
        /// </summary>
        IValue Value { get; }

    }
}
