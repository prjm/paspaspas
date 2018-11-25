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
        /// <param name="byteBoolValue"></param>
        public ByteBooleanValue(byte byteBoolValue)
            => Value = byteBoolValue;

        /// <summary>
        ///     fixed type id
        /// </summary>
        public override int TypeId
            => KnownTypeIds.ByteBoolType;

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
    }
}
