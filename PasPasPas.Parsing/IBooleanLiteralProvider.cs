namespace PasPasPas.Parsing {

    /// <summary>
    ///     boolean literals
    /// </summary>
    public interface IBooleanLiteralProvider {

        /// <summary>
        ///     true value
        /// </summary>
        object TrueValue { get; }

        /// <summary>
        ///     false value
        /// </summary>
        object FalseValue { get; }
    }
}