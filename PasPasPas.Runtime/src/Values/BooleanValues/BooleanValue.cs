using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     boolean value
    /// </summary>
    public class BooleanValue : BooleanValueBase {

        private readonly bool value;

        /// <summary>
        ///     create a new boolean value
        /// </summary>
        /// <param name="aValue"></param>
        public BooleanValue(bool aValue)
            => value = aValue;

        /// <summary>
        ///     type id
        /// </summary>
        public override int TypeId
            => KnownTypeIds.BooleanType;

        /// <summary>
        ///     get the boolean value
        /// </summary>
        public override bool AsBoolean
            => value;

        /// <summary>
        ///     format this boolean as string
        /// </summary>
        /// <returns></returns>
        public override string InternalTypeFormat
            => StringUtils.Invariant($"{value}");

        /// <summary>
        ///     get the integral value of this boolean
        /// </summary>
        public override uint AsUint
            => value ? uint.MaxValue : 0;

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override ITypeReference GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(value ? 1 : 0);
    }
}
