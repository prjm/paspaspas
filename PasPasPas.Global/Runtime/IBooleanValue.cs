namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     boolean values
    /// </summary>
    public interface IBooleanValue : IValue {

        /// <summary>
        ///     get the boolean value
        /// </summary>
        bool AsBoolean { get; }
    }
}
