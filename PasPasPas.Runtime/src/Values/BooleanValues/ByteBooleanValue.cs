using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     constant byte boolean (8 bit)
    /// </summary>
    public class ByteBooleanValue : BooleanValueBase {

        /// <summary>
        ///     create a new byte boolean
        /// </summary>
        /// <param name="byteBoolValue">boolean value</param>
        /// <param name="typeId">type id</param>
        public ByteBooleanValue(byte byteBoolValue, int typeId) {
            Value = byteBoolValue;
            TypeId = typeId;
        }

        /// <summary>
        ///     fixed type id
        /// </summary>
        public override int TypeId { get; }

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
        ///     convert this type to a string
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
        public override ITypeReference GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value);
    }
}
