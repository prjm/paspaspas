#nullable disable
using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

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
        /// <param name="typeDefinition"></param>
        public Int64Value(ITypeDefinition typeDefinition, long withValue) : base(typeDefinition, IntegralTypeKind.Int64)
            => value = withValue;


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
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => unchecked(((int)value) ^ (int)(value >> 32));

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
            => (uint)value;

    }
}
