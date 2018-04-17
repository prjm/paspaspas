﻿using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.Char {

    /// <summary>
    ///     wide char value
    /// </summary>
    public class WideCharValue : CharValueBase, IEquatable<WideCharValue> {

        private readonly char value;

        /// <summary>
        ///     create a new wide char value
        /// </summary>
        /// <param name="character"></param>
        public WideCharValue(char character)
            => value = character;

        /// <summary>
        ///     type id
        /// </summary>
        public override int TypeId
            => KnownTypeIds.WideCharType;

        /// <summary>
        ///     char value
        /// </summary>
        public override char AsWideChar
            => value;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => obj is WideCharValue charValue && charValue.value == value;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(WideCharValue other)
            => value == other.value;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => value.GetHashCode();

        /// <summary>
        ///     convert this value to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => value.ToString();
    }
}
