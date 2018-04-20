﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Global.Constants;

namespace PasPasPas.Runtime.Values.Int {

    /// <summary>
    ///     cardinal value
    /// </summary>
    public class CardinalValue : IntegerValueBase {

        private readonly uint value;

        /// <summary>
        ///     create a new cardinal value
        /// </summary>
        /// <param name="value"></param>
        public CardinalValue(uint value)
            => this.value = value;

        /// <summary>
        ///     type id
        /// </summary>
        public override int TypeId
            => KnownTypeIds.CardinalType;

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
            if (obj is CardinalValue v)
                return v.value == value;
            return false;
        }
        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => (int)value;

        /// <summary>
        ///     check if this number is negative
        /// </summary>
        public override bool IsNegative
            => value < 0;

        /// <summary>
        ///     unsigned value
        /// </summary>
        public override ulong UnsignedValue
            => value;


    }
}