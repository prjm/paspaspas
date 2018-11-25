using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.IntValues {

    /// <summary>
    ///     byte value (8 bit integer, unsigned)
    /// </summary>
    public class ByteValue : IntegerValueBase {

        private readonly byte value;

        /// <summary>
        ///     create a new byte value
        /// </summary>
        /// <param name="value"></param>
        public ByteValue(byte value)
            => this.value = value;

        /// <summary>
        ///     type id
        /// </summary>
        public override int TypeId
            => KnownTypeIds.ByteType;

        /// <summary>
        ///     byte value
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
            => StringUtils.Invariant($"{value}");

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => value;

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
            => value;

        /// <summary>
        ///     common type kind: integer
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.IntegerType;

        /// <summary>
        ///     convert this value to an internal string format
        /// </summary>
        public override string InternalTypeFormat
            => $"{value}";

    }
}
