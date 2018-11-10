namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for array types
    /// </summary>
    public interface IArrayValue : ITypeReference {

        /// <summary>
        ///     array base type
        /// </summary>
        int BaseType { get; }

    }
}
