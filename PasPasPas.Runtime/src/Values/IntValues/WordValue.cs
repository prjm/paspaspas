#nullable disable
using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.IntValues {

    /// <summary>
    ///     word value
    /// </summary>
    public class WordValue : IntegerValueBase {

        private readonly ushort value;

        /// <summary>
        ///     create a new word value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeDef"></param>
        public WordValue(ITypeDefinition typeDef, ushort value) : base(typeDef, IntegralTypeKind.Word)
            => this.value = value;

        /// <summary>
        ///     word value
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
