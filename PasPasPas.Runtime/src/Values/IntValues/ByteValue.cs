using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

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
        /// <param name="typeDef"></param>
        public ByteValue(ITypeDefinition typeDef, byte value) : base(typeDef, IntegralTypeKind.Byte)
            => this.value = value;

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
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => value;

        /// <summary>
        ///     invert all bits
        /// </summary>
        /// <returns></returns>
        public override IValue InvertBits()
            => ToScaledIntegerValue(~value);

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types)
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

    }
}
