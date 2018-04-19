namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     interface for runtime values
    /// </summary>
    public interface IValue {

        /// <summary>
        ///     get the well-known type id of this value
        /// </summary>
        /// <see cref="Constants.KnownTypeIds"/>
        int TypeId { get; }

    }
}
