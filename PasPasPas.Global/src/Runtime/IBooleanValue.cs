using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     boolean runtime values
    /// </summary>
    public interface IBooleanValue : IValue {

        /// <summary>
        ///     get the boolean value
        /// </summary>
        bool AsBoolean { get; }

        /// <summary>
        ///     get the boolean value as integer
        /// </summary>
        uint AsUint { get; }

        /// <summary>
        ///     type kind
        /// </summary>
        BooleanTypeKind Kind { get; }
    }
}
