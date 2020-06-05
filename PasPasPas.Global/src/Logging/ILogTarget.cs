#nullable disable
namespace PasPasPas.Globals.Log {

    /// <summary>
    ///     target for log messages
    /// </summary>
    public interface ILogTarget {

        /// <summary>
        ///     unregister this target
        /// </summary>
        /// <param name="logManager"></param>
        void UnregisteredAt(ILogManager logManager);

        /// <summary>
        ///     register this target
        /// </summary>
        /// <param name="logManager"></param>
        void RegisteredAt(ILogManager logManager);

        /// <summary>
        ///     handle a message
        /// </summary>
        /// <param name="message">message</param>
        void HandleMessage(ILogMessage message);
    }
}