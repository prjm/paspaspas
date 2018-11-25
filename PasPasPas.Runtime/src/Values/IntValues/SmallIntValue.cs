using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.IntValues {

    /// <summary>
    ///     small int value
    /// </summary>
    public class SmallIntValue : IntegerValueBase {

        private readonly short value;

        /// <summary>
        ///     create a new small int value
        /// </summary>
        /// <param name="value">value</param>
        public SmallIntValue(short value)
            => this.value = value;

        /// <summary>
        ///     type id: small int
        /// </summary>
        public override int TypeId
            => KnownTypeIds.SmallInt;

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
            => value;

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
            => (ulong)value;

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.IntegerType;
    }
}
