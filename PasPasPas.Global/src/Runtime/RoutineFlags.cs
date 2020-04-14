using System;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     routine flags
    /// </summary>
    [Flags]
    public enum RoutineFlags {

        /// <summary>
        ///     no flag
        /// </summary>
        None = 0b_0000,

        /// <summary>
        ///     class item
        /// </summary>
        ClassItem = 0b_0001,

    }
}