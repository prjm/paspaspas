using System.Numerics;
using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;

namespace PasPasPas.Runtime.Values.Int {

    /// <summary>
    ///     int64 value
    /// </summary>
    public class Int64Value : IntegerValueBase {

        private readonly long value;

        /// <summary>
        ///     creates a new int64 value
        /// </summary>
        /// <param name="withValue"></param>
        public Int64Value(long withValue)
            => value = withValue;

        /// <summary>
        ///     type id: int64
        /// </summary>
        public override int TypeId
            => KnownTypeIds.Int64Type;

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
            if (obj is Int64Value v)
                return v.value == value;
            return false;
        }
        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => (unchecked((int)value) ^ (int)(value >> 32));

        /// <summary>
        ///     invert all bits
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
            => (uint)value;

        /// <summary>
        ///     type kind: int64
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.Int64Type;
    }
}
