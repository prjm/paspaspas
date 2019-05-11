using System;
using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;

namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     route a message
    /// </summary>
    public class LogManager : ILogManager, IEnvironmentItem {

        /// <summary>
        ///     log targets
        /// </summary>
        private readonly IList<ILogTarget> targets
            = new List<ILogTarget>();

        /// <summary>
        ///     count
        /// </summary>
        public int Count
            => targets.Count;

        /// <summary>
        ///     manager caption
        /// </summary>
        public static string Caption
            => "LogManager";

        /// <summary>
        ///     register target
        /// </summary>
        /// <param name="target"></param>
        public void RegisterTarget(ILogTarget target) {

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            targets.Add(target);
            target.RegisteredAt(this);
        }

        /// <summary>
        ///     route a message
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message to rouce</param>
        public void RouteMessage(ILogSource source, ILogMessage message) {

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            foreach (var target in targets)
                target.HandleMessage(message);
        }

        /// <summary>
        ///     unregisters a target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool UnregisterTarget(ILogTarget target) {

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var found = false;

            for (var i = targets.Count - 1; i >= 0; i--) {
                var currentTarget = targets[i];
                if (currentTarget == target) {
                    target.UnregisteredAt(this);
                    targets.RemoveAt(i);
                    found = true;
                };
            }

            return found;
        }

        /// <summary>
        ///     create a new log source
        /// </summary>
        /// <param name="logGroup"></param>
        /// <returns></returns>
        public ILogSource CreateLogSource(uint logGroup)
            => new LogSource(this, logGroup);
    }
}
