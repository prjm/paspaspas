﻿#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     generic constraint kind
    /// </summary>
    public enum GenericConstraintKind : byte {
        /// <summary>
        ///     unknown constraint
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// record constraint
        /// </summary>
        Record = 1,

        /// <summary>
        ///     class constraint
        /// </summary>
        Class = 2,

        /// <summary>
        ///     constructor constraint
        /// </summary>
        Constructor = 3,

        /// <summary>
        ///     identifier constraint
        /// </summary>
        Identifier = 4,
    }
}
