using System;
using PasPasPas.Globals.Runtime;

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

        /// <summary>
        ///     read a constant value
        /// </summary>
        /// <returns></returns>
        IValue ReadConstant(IStringRegistry strings);

        /// <summary>
        ///     read string values
        /// </summary>
        /// <param name="strings"></param>
        void ReadStrings(IStringRegistry strings);
    }
}
