﻿using System;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     base class for char values
    /// </summary>
    public abstract class CharValueBase : ICharValue, IStringValue, IEquatable<ICharValue>, IEquatable<IStringValue> {

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
        ///     <c>true</c> for all constant char values
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     type kind
        /// </summary>
        public abstract CommonTypeKind TypeKind { get; }

        /// <summary>
        ///     test for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ICharValue charValue)
                return Equals(charValue);

            if (obj is IStringValue stringValue)
                return Equals(stringValue);


            return false;
        }

        /// <summary>
        ///     test for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ICharValue other)
            => other.AsWideChar == AsWideChar;

        /// <summary>
        ///     test for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IStringValue other)
            => string.Equals(other.AsUnicodeString, AsUnicodeString, StringComparison.Ordinal);

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