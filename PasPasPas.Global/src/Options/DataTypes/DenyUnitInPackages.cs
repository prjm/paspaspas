﻿#nullable disable
namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     flag to disallow unit in packages
    /// </summary>
    public enum DenyUnitInPackage {

        /// <summary>
        ///     undefined status
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     allow unit
        /// </summary>
        AllowUnit = 1,

        /// <summary>
        ///     deny unit
        /// </summary>
        DenyUnit = 2
    }
}