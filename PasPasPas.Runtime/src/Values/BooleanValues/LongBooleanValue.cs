using System.Globalization;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     long boolean value
    /// </summary>
    internal class LongBooleanValue : BooleanValueBase {

        /// <summary>
        ///     create a new long boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeDef"></param>
        internal LongBooleanValue(uint value, ITypeDefinition typeDef) : base(typeDef, BooleanTypeKind.LongBool)
            => Value = value;

        /// <summary>
        ///     convert this value as boolean
        /// </summary>
        public override bool AsBoolean
            => Value != 0;

        /// <summary>
        ///     value
        /// </summary>
        public uint Value { get; }

        /// <summary>
        ///     get the value of this boolean
        /// </summary>
        public override uint AsUint
            => Value;

        public override bool Equals(IValue? other)
            => other is LongBooleanValue v && v.Value == Value;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => unchecked((int)Value);

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value);

        /// <summary>
        ///     get a value string
        /// </summary>
        /// <returns></returns>
        public override string GetValueString()
            => Value.ToString(CultureInfo.InvariantCulture);
    }
}
