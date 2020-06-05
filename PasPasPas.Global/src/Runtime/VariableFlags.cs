#nullable disable
using System;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     variable flags
    /// </summary>
    [Flags]
    public enum VariableFlags : byte {

        /// <summary>
        ///     no flags
        /// </summary>
        None = 0b_0,


        /// <summary>
        ///     class item
        /// </summary>
        ClassItem = 0b_1,
    }
}
