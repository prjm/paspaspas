namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for parameter groups
    /// </summary>
    public interface IParameterGroup {

        /// <summary>
        ///     result type
        /// </summary>
        ITypeReference ResultType { get; }
    }
}
