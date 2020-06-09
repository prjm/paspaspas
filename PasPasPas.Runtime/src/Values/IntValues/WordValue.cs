using System.Globalization;
using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.IntValues {

    /// <summary>
    ///     word value
    /// </summary>
    internal class WordValue : IntegerValueBase {

        private readonly ushort wordValue;

        /// <summary>
        ///     create a new word value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeDef"></param>
        public WordValue(ITypeDefinition typeDef, ushort value) : base(typeDef, IntegralTypeKind.Word)
            => wordValue = value;

        /// <summary>
        ///     word value
        /// </summary>
        public override long SignedValue
            => wordValue;

        /// <summary>
        ///     get this value as big integer value
        /// </summary>
        public override BigInteger AsBigInteger
            => new BigInteger(wordValue);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => wordValue;

        /// <summary>
        ///     invert bits
        /// </summary>
        /// <returns></returns>
        public override IValue InvertBits()
            => ToScaledIntegerValue(~wordValue);

        /// <summary>
        ///     get the ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types)
            => ToScaledIntegerValue(wordValue);

        /// <summary>
        ///     check if this number is negative
        /// </summary>
        public override bool IsNegative
            => wordValue < 0;

        /// <summary>
        ///     unsigned value
        /// </summary>
        public override ulong UnsignedValue
            => wordValue;

        /// <summary>
        ///     convert this value to a value string
        /// </summary>
        /// <returns></returns>
        public override string GetValueString()
            => wordValue.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(IValue? other)
            => other is WordValue w && w.wordValue == wordValue;
    }
}
