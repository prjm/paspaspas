using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.IntValues {

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
        public override string InternalTypeFormat
            => StringUtils.Invariant($"{value}");

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => unchecked(((int)value) ^ (int)(value >> 32));

        /// <summary>
        ///     invert all bits
        /// </summary>
        /// <returns></returns>
        public override IOldTypeReference InvertBits()
            => ToScaledIntegerValue(~value);

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IOldTypeReference GetOrdinalValue(ITypeRegistry types)
            => ToScaledIntegerValue(value);

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
