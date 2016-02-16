namespace PasPasPas.Internal.Log {

    /// <summary>
    ///     log message targer
    /// </summary>
    public interface ILogTarget {

        /// <summary>
        ///     process a log message
        /// </summary>
        /// <param name="message">message to process</param>
        void ProcessMessage(LogMessage message);

    }
}