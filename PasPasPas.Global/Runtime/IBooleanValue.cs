namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     boolean runtime values
    /// </summary>
    public interface IBooleanValue : IValue {

        /// <summary>
        ///     get the boolean value
        /// </summary>
        bool AsBoolean { get; }

    }
}
