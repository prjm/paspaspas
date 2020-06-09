using System.Globalization;
using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.IntValues {

    /// <summary>
    ///     cardinal value
    /// </summary>
    internal class CardinalValue : IntegerValueBase {

        private readonly uint cardinalValue;

        /// <summary>
        ///     create a new cardinal value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeDef"></param>
        internal CardinalValue(ITypeDefinition typeDef, uint value) : base(typeDef, IntegralTypeKind.Cardinal)
            => this.cardinalValue = value;

        /// <summary>
        ///     value
        /// </summary>
        public override long SignedValue
            => cardinalValue;

        /// <summary>
        ///     get this value as big integer value
        /// </summary>
        public override BigInteger AsBigInteger
            => new BigInteger(cardinalValue);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => (int)cardinalValue;

        /// <summary>
        ///     invert bits
        /// </summary>
        /// <returns></returns>
        public override IValue InvertBits()
            => ToScaledIntegerValue(~cardinalValue);

        /// <summary>
        ///     get the ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types)
            => ToScaledIntegerValue(cardinalValue);

        public override string GetValueString()
            => cardinalValue.ToString(CultureInfo.InvariantCulture);

        public override bool Equals(IValue? other)
            => other is CardinalValue c && c.cardinalValue == cardinalValue;

        /// <summary>
        ///     check if this number is negative
        /// </summary>
        public override bool IsNegative
            => cardinalValue < 0;

        /// <summary>
        ///     unsigned value
        /// </summary>
        public override ulong UnsignedValue
            => cardinalValue;
    }
}
