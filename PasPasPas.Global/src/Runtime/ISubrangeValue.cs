namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     subrange value
    /// </summary>
    public interface ISubrangeValue : IValue, IOrdinalValue {

        /// <summary>
        ///     wrapped value
        /// </summary>
        IValue Value { get; }

    }
}
