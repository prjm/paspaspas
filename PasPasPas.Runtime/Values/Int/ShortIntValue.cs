﻿using System.Numerics;
using PasPasPas.Global.Constants;

namespace PasPasPas.Runtime.Values.Int {

    /// <summary>
    ///     short int value
    /// </summary>
    public class ShortIntValue : IntegerValueBase {

        private readonly sbyte value;

        /// <summary>
        ///     create a new short int value
        /// </summary>
        /// <param name="value"></param>
        public ShortIntValue(sbyte value)
            => this.value = value;

        /// <summary>
        ///     type id: short int
        /// </summary>
        public override int TypeId
            => KnownTypeIds.ShortInt;

        /// <summary>
        ///     value
        /// </summary>
        public override long SignedValue
            => value;

        /// <summary>
        ///     get this value as big integer value
        /// </summary>
        public override BigInteger AsBigInteger
            => new BigInteger(value);

        /// <summary>
        ///     format this number
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => value.ToString();

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ShortIntValue v)
                return v.value == value;
            return false;
        }
        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => value;

        /// <summary>
        ///     check if this number is negative
        /// </summary>
        public override bool IsNegative
            => value < 0;

        /// <summary>
        ///     unsigned value
        /// </summary>
        public override ulong UnsignedValue
            => (ulong)value;

    }
}