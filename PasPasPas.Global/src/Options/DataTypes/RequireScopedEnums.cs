﻿#nullable disable
namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     flag to required scoped enumerations
    /// </summary>
    public enum RequireScopedEnumMode {

        /// <summary>
        ///     undefined flag state
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     scope qualifier not required
        /// </summary>
        Disable = 1,

        /// <summary>
        ///     scope qualifier required
        /// </summary>
        Enable = 2,

    }
}