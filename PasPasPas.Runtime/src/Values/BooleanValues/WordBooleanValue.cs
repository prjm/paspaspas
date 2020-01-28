using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     word (16 bit) boolean value
    /// </summary>
    public class WordBooleanValue : BooleanValueBase {

        /// <summary>
        ///     create a new word boolean
        /// </summary>
        /// <param name="wordBoolValue"></param>
        /// <param name="typeDef">type id</param>
        public WordBooleanValue(ushort wordBoolValue, ITypeDefinition typeDef) : base(typeDef)
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
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IValue GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value);
    }
}
