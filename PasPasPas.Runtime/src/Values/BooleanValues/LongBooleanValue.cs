using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     long boolean value
    /// </summary>
    public class LongBooleanValue : BooleanValueBase {

        /// <summary>
        ///     create a new long boolean value
        /// </summary>
        /// <param name="value"></param>
        public LongBooleanValue(uint value)
            => Value = value;

        /// <summary>
        ///     type id
        /// </summary>
        public override int TypeId
            => KnownTypeIds.LongBoolType;

        /// <summary>
        ///     convert this value as boolean
        /// </summary>
        public override bool AsBoolean
            => Value != 0;

        /// <summary>
        ///     format  this value
        /// </summary>
        public override string InternalTypeFormat
            => StringUtils.Invariant($"{AsBoolean} ({Value})");

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
        public override ITypeReference GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value);
    }
}
