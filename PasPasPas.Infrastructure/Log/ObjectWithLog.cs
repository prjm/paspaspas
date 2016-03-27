using PasPasPas.Infrastructure.Service;

namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     base class for object with logging facilitiers
    /// </summary>
    public static class ObjectWithLog {

        /// <summary>
        ///     create an error message
        /// </summary>
        /// <param name="messageNumber">message number</param>
        /// <param name="baseObject">base object</param>
        /// <param name="data">additional information</param>
        public static void Error(this IObjectBase baseObject, int messageNumber, params object[] data) {
            var logServices = baseObject.Services.Resolve(StandardServices.LoggingServiceClass, baseObject) as ILogService;

            if (logServices == null)
                return;

            logServices.Log(new LogMessage(messageNumber, data));
        }

    }
}
