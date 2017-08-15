using System;

namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     interface for log event generators
    /// </summary>
    public interface ILogSource {

        /// <summary>
        ///     log an error message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        void Error(Guid id, params object[] data);

        /// <summary>
        ///     process a generated message
        /// </summary>
        /// <param name="message">log message</param>
        void ProcessMessage(ILogMessage message);

    }
}
