namespace PasPasPas.Internal.Log {

    /// <summary>
    ///     base class for object with logging facilitiers
    /// </summary>
    public class ObjectWithLog {

        /// <summary>
        ///     log target (aggregator)
        /// </summary>
        public ILogService LogTarget { get; set; }

    }
}
