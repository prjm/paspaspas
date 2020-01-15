using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     word (16 bit) boolean value
    /// </summary>
    public class WordBooleanValue : BooleanValueBase {

        /// <summary>
        ///     create a new word bool
        /// </summary>
        /// <param name="wordBoolValue"></param>
        /// <param name="typeId">type id</param>
        public WordBooleanValue(ushort wordBoolValue, int typeId) : base(typeId)
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
        ///     convert this value to string
        /// </summary>
        /// <returns></returns>
        public override string InternalTypeFormat
            => StringUtils.Invariant($"{AsBoolean} ({Value})");

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
        public override IOldTypeReference GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value);
    }
}
