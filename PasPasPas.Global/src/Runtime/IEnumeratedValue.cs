namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     value of an enumeration
    /// </summary>
    public interface IEnumeratedValue : IOldTypeReference {

        /// <summary>
        ///     constant value
        /// </summary>
        IOldTypeReference Value { get; }

    }
}
