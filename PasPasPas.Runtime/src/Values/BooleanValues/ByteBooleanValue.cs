using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     constant byte boolean (8 bit)
    /// </summary>
    public class ByteBooleanValue : BooleanValueBase {

        /// <summary>
        ///     create a new byte boolean
        /// </summary>
        /// <param name="byteBoolValue">boolean value</param>
        /// <param name="typeDefinition">type def</param>
        public ByteBooleanValue(byte byteBoolValue, ITypeDefinition typeDefinition) : base(typeDefinition)
            => Value = byteBoolValue;

        /// <summary>
        ///     boolean value
        /// </summary>
        public override bool AsBoolean
            => Value != 0;

        /// <summary>
        ///     boolean value
        /// </summary>
        public byte Value { get; }

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
