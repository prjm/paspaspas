namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     common interface for value types
    /// </summary>
    public interface IValue {

        /// <summary>
        ///     get the data of this value
        /// </summary>
        byte[] Data { get; }

        /// <summary>
        ///     get the common type id of this vlue
        /// </summary>
        int TypeId { get; }
    }
}
