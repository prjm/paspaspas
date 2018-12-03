namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     subrange value
    /// </summary>
    public interface ISubrangeValue : ITypeReference {

        /// <summary>
        ///     wrapped value
        /// </summary>
        ITypeReference Value { get; }

    }
}
