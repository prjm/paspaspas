﻿using System.Globalization;
using System.Numerics;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.IntValues {

    /// <summary>
    ///     small int value
    /// </summary>
    internal class SmallIntValue : IntegerValueBase {

        private readonly short value;

        /// <summary>
        ///     create a new small int value
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="typeDef"></param>
        public SmallIntValue(ITypeDefinition typeDef, short value) : base(typeDef, IntegralTypeKind.SmallInt)
            => this.value = value;

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
        ///     get this value as string
        /// </summary>
        /// <returns></returns>
        public override string GetValueString()
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(IValue? other)
            => other is SmallIntValue s && s.value == value;

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

    }
}
