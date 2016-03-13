namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     log message targer
    /// </summary>
    public interface ILogService {

        /// <summary>
        ///     process a log message
        /// </summary>
        /// <param name="message">message to process</param>
        void Log(LogMessage message);

    }
}