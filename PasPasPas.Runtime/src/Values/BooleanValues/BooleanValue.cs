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
    }
}
