namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     value of an enumeration
    /// </summary>
    public interface IEnumeratedValue : ITypeReference {

        /// <summary>
        ///     constant value
        /// </summary>
        ITypeReference Value { get; }

    }
}
