using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     a simple execution timer
    /// </summary>
    public class ExecutionTimer {

        private long hitCount = 0;
        private long tickCount = 0;
        private long startTickCount;
        private int stackCount = 0;
        private TimeSpan duration = default;
        private string description = string.Empty;
        private ExecutionTimer parent = null;
        private Lazy<IList<ExecutionTimer>> children
            = new Lazy<IList<ExecutionTimer>>(() => new List<ExecutionTimer>());

        /// <summary>
        ///     number of hits
        /// </summary>
        public long HitCount
            => hitCount;

        /// <summary>
        ///     elapsed ticks
        /// </summary>
        public long TickCount
            => tickCount;

        /// <summary>
        ///     test if this timer is running
        /// </summary>
        public bool IsRunning
            => stackCount > 0;

        /// <summary>
        ///     summed duration
        /// </summary>
        public TimeSpan Duration
            => duration;

        /// <summary>
        ///     create a new execution timer
        /// </summary>
        /// <param name="parentTimer"></param>
        public ExecutionTimer(ExecutionTimer parentTimer = null) {
            parent = parentTimer;
            if (parent != null)
                parent.children.Value.Add(this);
        }

        /// <summary>
        ///     start the timer
        /// </summary>
        /// <returns><c>true</c> if the timer was not already running</returns>
        public bool Start(bool hit = true) {

            void startParents(ExecutionTimer timer) {
                if (timer.parent != null)
                    startParents(timer.parent);
                timer.Start(false);
            };

            if (parent != null)
                startParents(parent);

            if (stackCount > 0) {
                stackCount++;
                if (hit)
                    hitCount++;
                return false;
            }

            startTickCount = System.Environment.TickCount;
            stackCount++;
            if (hit)
                hitCount++;
            return true;
        }

        /// <summary>
        ///     stop the timer
        /// </summary>
        /// <returns><c>true</c> if the timer was not already stopped</returns>
        public bool Stop() {

            void stopParents(ExecutionTimer timer) {
                timer.Stop();
                if (timer.parent != null)
                    stopParents(timer.parent);
            };

            if (parent != null)
                stopParents(parent);

            if (stackCount > 1) {
                stackCount--;
                return false;
            }

            var ticks = System.Environment.TickCount - startTickCount;
            tickCount += ticks;
            stackCount = 0;
            duration = duration.Add(TimeSpan.FromTicks(ticks));
            return true;
        }


    }
}
