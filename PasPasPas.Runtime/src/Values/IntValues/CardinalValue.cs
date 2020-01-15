using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.IntValues {

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
            => StringUtils.Invariant($"{value}");

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => (int)value;

        /// <summary>
        ///     invert bits
        /// </summary>
        /// <returns></returns>
        public override IOldTypeReference InvertBits()
            => ToScaledIntegerValue(~value);

        /// <summary>
        ///     get the ordinal value
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
