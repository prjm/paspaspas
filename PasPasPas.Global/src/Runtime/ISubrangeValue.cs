#nullable disable
namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     subrange value
    /// </summary>
    public interface ISubrangeValue : IOrdinalValue {

        /// <summary>
        ///     wrapped value
        /// </summary>
        IValue WrappedValue { get; }

    }
}
