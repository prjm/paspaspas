namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     interface for value types
    /// </summary>
    public interface IValue {

        /// <summary>
        ///     get the data of this value
        /// </summary>
        byte[] Data { get; }

        /// <summary>
        ///     get the well-known type id of this value
        /// </summary>
        int TypeId { get; }
    }
}
