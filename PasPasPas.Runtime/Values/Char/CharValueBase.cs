﻿using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.Char {

    /// <summary>
    ///     base class for char values
    /// </summary>
    public abstract class CharValueBase : ICharValue, IStringValue {

        /// <summary>
        ///     get the type id
        /// </summary>
        public abstract int TypeId { get; }

        /// <summary>
        ///     get the boolean value
        /// </summary>
        public abstract char AsWideChar { get; }

        /// <summary>
        ///     convert this char to a string
        /// </summary>
        public string AsUnicodeString
            => new string(AsWideChar, 1);

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract override bool Equals(object obj);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public abstract override int GetHashCode();

        /// <summary>
        ///     format this number as string
        /// </summary>
        /// <returns>number as string</returns>
        public abstract override string ToString();



    }
}