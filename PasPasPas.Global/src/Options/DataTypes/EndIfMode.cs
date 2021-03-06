﻿#nullable disable
namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///    <c>if</c> / <c>ifend</c> mode
    /// </summary>
    public enum EndIfMode {

        /// <summary>
        ///     undefined flag state
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     enable legacy mode
        /// </summary>
        LegacyIfEnd = 1,

        /// <summary>
        ///     modern standard mode
        /// </summary>
        Standard = 2,
    }
}