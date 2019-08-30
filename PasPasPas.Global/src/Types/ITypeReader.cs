using System;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     read types
    /// </summary>
    public interface ITypeReader : IDisposable {

        /// <summary>
        ///     read a unit
        /// </summary>
        /// <returns></returns>
        ITypeDefinition ReadUnit();
    }
}
