namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     interface for log event generators
    /// </summary>
    public interface ILogSource {

        /// <summary>
        ///     process a generated message
        /// </summary>
        /// <param name="message">log message</param>
        void ProcessMessage(ILogMessage message);

    }
}
