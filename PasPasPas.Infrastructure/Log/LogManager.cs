using System;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Log {

    /// <summary>
    ///     route a message
    /// </summary>
    public class LogManager : ILogManager {

        /// <summary>
        ///     log targets
        /// </summary>
        private IList<ILogTarget> targets
            = new List<ILogTarget>();


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

            foreach (ILogTarget target in targets)
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

            bool found = false;

            for (int i = targets.Count - 1; i >= 0; i--) {
                ILogTarget currentTarget = targets[i];
                if (currentTarget == target) {
                    target.UnregisteredAt(this);
                    targets.RemoveAt(i);
                    found = true;
                };
            }

            return found;
        }
    }
}
