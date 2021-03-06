﻿#nullable disable
namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     flag for overflow checks
    /// </summary>
    public enum RuntimeOverflowCheck {


        /// <summary>
        ///     undefined flag state
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     enable checks
        /// </summary>
        EnableChecks = 1,

        /// <summary>
        ///     disable checks
        /// </summary>
        DisableChecks = 2,
    }
}