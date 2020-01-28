using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     support for basic boolean values
    /// </summary>
    public class BooleanValue : BooleanValueBase {

        /// <summary>
        ///     create a new boolean value
        /// </summary>
        /// <param name="aValue">boolean value</param>
        /// <param name="typeDef">type definition</param>
        public BooleanValue(bool aValue, ITypeDefinition typeDef) : base(typeDef, BooleanTypeKind.Boolean)
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
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value ? 1 : 0);
    }
}
