namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     interface for options
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOption<T> {

        /// <summary>
        ///     option value
        /// </summary>
        T Value { get; set; }

        /// <summary>
        ///     reset option value
        /// </summary>
        void ResetToDefault();
    }
}
