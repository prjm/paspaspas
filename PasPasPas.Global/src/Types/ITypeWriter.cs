using System;
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

        /// <summary>
        ///     write a constant value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="strings">string registry</param>
        void WriteConstant(IValue value, IStringRegistry strings);

        /// <summary>
        ///     write string values
        /// </summary>
        /// <param name="strings"></param>
        void WriteStrings(IStringRegistry strings);

        /// <summary>
        ///     prepare constants
        /// </summary>
        /// <param name="value"></param>
        /// <param name="strings"></param>
        void PrepareConstant(IValue value, IStringRegistry strings);
    }
}
