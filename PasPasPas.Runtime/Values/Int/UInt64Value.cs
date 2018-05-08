using System.Numerics;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values.Int {

    /// <summary>
    ///     unsigned int64 value
    /// </summary>
    public class UInt64Value : IntegerValueBase {

        private readonly ulong value;

        /// <summary>
        ///     create a new unsigned long value
        /// </summary>
        /// <param name="value"></param>
        public UInt64Value(ulong value)
            => this.value = value;

        /// <summary>
        ///     type id: uint64
        /// </summary>
        public override int TypeId
            => KnownTypeIds.Uint64Type;

        /// <summary>
        ///     value
        /// </summary>
        public override long SignedValue
            => (long)value;


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
            if (obj is UInt64Value v)
                return v.value == value;
            return false;
        }
        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => ((int)value) ^ (int)(value >> 32);

        /// <summary>
        ///     invert bits
        /// </summary>
        /// <returns></returns>
        public override ITypeReference InvertBits()
            => ToScaledIntegerValue(~value);

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

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.Int64Type;
    }
}
