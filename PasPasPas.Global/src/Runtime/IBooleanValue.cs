namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     boolean runtime values
    /// </summary>
    public interface IBooleanValue : ITypeReference {

        /// <summary>
        ///     get the boolean value
        /// </summary>
        bool AsBoolean { get; }

        /// <summary>
        ///     get the boolean value as integer
        /// </summary>
        uint AsUint { get; }

    }
}
