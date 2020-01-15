namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     subrange value
    /// </summary>
    public interface ISubrangeValue : IOldTypeReference, IOrdinalValue {

        /// <summary>
        ///     wrapped value
        /// </summary>
        IOldTypeReference Value { get; }

    }
}
