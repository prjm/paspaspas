#nullable disable
namespace PasPasPas.Globals.Log {

    /// <summary>
    ///     interface for log event generators
    /// </summary>
    public interface ILogSource {

        /// <summary>
        ///     log manager
        /// </summary>
        ILogManager Manager { get; }

        /// <summary>
        ///     create a warning message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        void LogFatalError(uint id, params object[] data);

        /// <summary>
        ///     log an error message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        void LogError(uint id, params object[] data);

        /// <summary>
        ///     create a warning message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        void LogWarning(uint id, params object[] data);

        /// <summary>
        ///     create a hint message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        void LogHint(uint id, params object[] data);

        /// <summary>
        ///     log a debug message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        void LogDebug(uint id, params object[] data);
    }
}
