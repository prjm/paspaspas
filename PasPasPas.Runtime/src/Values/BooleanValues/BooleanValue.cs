using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     support for basic boolean values
    /// </summary>
    internal class BooleanValue : BooleanValueBase {

        /// <summary>
        ///     create a new boolean value
        /// </summary>
        /// <param name="aValue">boolean value</param>
        /// <param name="typeDef">type definition</param>
        internal BooleanValue(bool aValue, ITypeDefinition typeDef) : base(typeDef, BooleanTypeKind.Boolean)
            => Value = aValue;

        /// <summary>
        ///     get the boolean value
        /// </summary>
        public override bool AsBoolean
            => Value;

        /// <summary>
        ///     get the integral value of this boolean
        /// </summary>
        public override uint AsUint
            => Value ? uint.MaxValue : 0;

        /// <summary>
        ///     value
        /// </summary>
        public bool Value { get; }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(IValue? other)
            => other is IBooleanValue b &&
                b.Kind == Kind &&
                b.AsUint == AsUint;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => Value ? 1 : 0;

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value ? 1 : 0);

        /// <summary>
        ///     get the value string
        /// </summary>
        /// <returns></returns>
        public override string GetValueString()
            => Value ? KnownNames.True : KnownNames.False;
    }
}
