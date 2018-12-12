using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.IntValues {

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
        public override string InternalTypeFormat
            => StringUtils.Invariant($"{value}");

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
        ///     get the ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override ITypeReference GetOrdinalValue(ITypeRegistry types)
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
            => value;

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.Int64Type;
    }
}
