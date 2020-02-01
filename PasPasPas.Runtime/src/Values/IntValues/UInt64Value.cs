using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

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
        /// <param name="typeDef"></param>
        public UInt64Value(ITypeDefinition typeDef, ulong value) : base(typeDef, IntegralTypeKind.UInt64)
            => this.value = value;

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
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => ((int)value) ^ (int)(value >> 32);

        /// <summary>
        ///     invert bits
        /// </summary>
        /// <returns></returns>
        public override IValue InvertBits()
            => ToScaledIntegerValue(~value);

        /// <summary>
        ///     get the ordinal value
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
