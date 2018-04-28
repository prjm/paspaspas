using System.Numerics;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.Int {

    /// <summary>
    ///     integer value
    /// </summary>
    public class IntegerValue : IntegerValueBase {

        private readonly int value;

        /// <summary>
        ///     new int value
        /// </summary>
        /// <param name="value"></param>
        public IntegerValue(int value)
            => this.value = value;

        /// <summary>
        ///     type id: integer
        /// </summary>
        public override int TypeId
            => KnownTypeIds.IntegerType;

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
        ///     check if this number is negative
        /// </summary>
        public override bool IsNegative
            => value < 0;

        /// <summary>
        ///     unsigned value
        /// </summary>
        public override ulong UnsignedValue
            => (uint)value;

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.IntegerType;

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
            if (obj is IntegerValue v)
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
        ///     invert bits
        /// </summary>
        /// <returns></returns>
        public override IValue InvertBits()
            => ToScaledIntegerValue(~value);

    }
}
