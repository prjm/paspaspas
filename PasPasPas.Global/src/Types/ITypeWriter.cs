﻿using System;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     write types
    /// </summary>
    public interface ITypeWriter : IDisposable {

        /// <summary>
        ///     write a unit
        /// </summary>
        /// <param name="unitType"></param>
        void WriteUnit(IUnitType unitType);
        void WriteConstant(IOldTypeReference value);
    }
}
