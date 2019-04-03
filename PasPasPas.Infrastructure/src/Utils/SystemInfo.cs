using System;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     small diagnostic system info
    /// </summary>
    public class SystemInfo {

        /// <summary>
        ///     create a new  system info
        /// </summary>
        public SystemInfo() {
            WorkingSet = GC.GetTotalMemory(false);
            CollectionCount0 = GC.CollectionCount(0);
            CollectionCount1 = GC.CollectionCount(1);
            CollectionCount2 = GC.CollectionCount(2);
        }

        /// <summary>
        ///     create a new system info and create a difference of the values
        /// </summary>
        /// <param name="olderValue"></param>
        public SystemInfo(SystemInfo olderValue) : this() {
            WorkingSet -= olderValue.WorkingSet;
            CollectionCount0 -= olderValue.CollectionCount0;
            CollectionCount1 -= olderValue.CollectionCount1;
            CollectionCount2 -= olderValue.CollectionCount2;
        }

        /// <summary>
        ///     working set (in bytes)
        /// </summary>
        public long WorkingSet { get; }

        /// <summary>
        ///     garbage collection count level 0
        /// </summary>
        public int CollectionCount0 { get; }

        /// <summary>
        ///     garbage collection count level 1
        /// </summary>
        public int CollectionCount1 { get; }

        /// <summary>
        ///     garbage collection count level 2
        /// </summary>
        public int CollectionCount2 { get; }
    }
}
