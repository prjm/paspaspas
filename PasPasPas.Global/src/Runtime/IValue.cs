using System;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     basic interface for constant values
    /// </summary>
    public interface IValue : ITypeSymbol, IEquatable<IValue> {

        /// <summary>
        ///     displays the value as a string, possibly cached
        /// </summary>
        /// <returns></returns>
        string ToValueString();

        /// <summary>
        ///     create the value as string (not cached)
        /// </summary>
        /// <returns></returns>
        string GetValueString();

        /// <summary>
        ///     compute a hash code for this value
        /// </summary>
        /// <returns></returns>
        int GetHashCode();

        /// <summary>
        ///     compare for equality
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Equals(object? value)
            => Equals(value as IValue);

    }
}
