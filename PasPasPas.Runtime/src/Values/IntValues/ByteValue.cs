using System.Globalization;
using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.IntValues {

    /// <summary>
    ///     byte value (8 bit integer, unsigned)
    /// </summary>
    internal class ByteValue : IntegerValueBase {

        private readonly byte byteValue;

        /// <summary>
        ///     create a new byte value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeDef"></param>
        internal ByteValue(ITypeDefinition typeDef, byte value) : base(typeDef, IntegralTypeKind.Byte)
            => this.byteValue = value;

        /// <summary>
        ///     byte value
        /// </summary>
        public override long SignedValue
            => byteValue;

        /// <summary>
        ///     get this value as big integer value
        /// </summary>
        public override BigInteger AsBigInteger
            => new BigInteger(byteValue);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => byteValue;

        /// <summary>
        ///     invert all bits
        /// </summary>
        /// <returns></returns>
        public override IValue InvertBits()
            => ToScaledIntegerValue(~byteValue);

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types)
            => ToScaledIntegerValue(byteValue);

        /// <summary>
        ///     convert this value to a string
        /// </summary>
        /// <returns></returns>
        public override string GetValueString()
            => byteValue.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        ///     compare to another value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(IValue? other)
            => other is ByteValue b && b.byteValue == byteValue;

        /// <summary>
        ///     check if this number is negative
        /// </summary>
        public override bool IsNegative
            => byteValue < 0;

        /// <summary>
        ///     unsigned value
        /// </summary>
        public override ulong UnsignedValue
            => byteValue;

    }
}
