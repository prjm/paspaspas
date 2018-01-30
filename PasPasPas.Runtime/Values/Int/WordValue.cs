using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.Int {

    /// <summary>
    ///     word value
    /// </summary>
    public class WordValue : IntegerValueBase {

        private readonly ushort value;

        /// <summary>
        ///     create a new word value
        /// </summary>
        /// <param name="value"></param>
        public WordValue(ushort value)
            => this.value = value;

        /// <summary>
        ///     type id: word
        /// </summary>
        public override int TypeId
            => KnownTypeIds.WordType;

        /// <summary>
        ///     word value
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
            if (obj is WordValue v)
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
            => value;

    }
}
