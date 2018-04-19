namespace PasPasPas.Global.Runtime {

    /// <summary>
    ///     widechar runtime value (utf-16)
    /// </summary>
    public interface ICharValue : IValue {

        /// <summary>
        ///     get the wide char value
        /// </summary>
        char AsWideChar { get; }

    }
}
