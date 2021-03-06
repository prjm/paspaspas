﻿using System.Globalization;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     word (16 bit) boolean value
    /// </summary>
    internal class WordBooleanValue : BooleanValueBase {

        /// <summary>
        ///     create a new word boolean
        /// </summary>
        /// <param name="wordBoolValue"></param>
        /// <param name="typeDef">type id</param>
        internal WordBooleanValue(ushort wordBoolValue, ITypeDefinition typeDef) : base(typeDef, BooleanTypeKind.WordBool)
            => Value = wordBoolValue;

        /// <summary>
        ///     boolean value
        /// </summary>
        public override bool AsBoolean
            => Value != 0;

        /// <summary>
        ///     word boolean value
        /// </summary>
        public ushort Value { get; }

        /// <summary>
        ///     get the value of this boolean
        /// </summary>
        public override uint AsUint
            => Value;

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(IValue? other)
            => other is WordBooleanValue w && w.Value == Value;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => Value;

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value);

        /// <summary>
        ///     get the value of this boolean as string
        /// </summary>
        /// <returns></returns>
        public override string GetValueString()
            => Value.ToString(CultureInfo.InvariantCulture);
    }
}
