namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     interface for value types
    /// </summary>
    public interface IValue {

        /// <summary>
        ///     get the well-known type id of this value
        /// </summary>
        int TypeId { get; }
    }
}
