#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     long boolean value
    /// </summary>
    public class LongBooleanValue : BooleanValueBase {

        /// <summary>
        ///     create a new long boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeDef"></param>
        public LongBooleanValue(uint value, ITypeDefinition typeDef) : base(typeDef, BooleanTypeKind.LongBool)
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

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value);
    }
}
