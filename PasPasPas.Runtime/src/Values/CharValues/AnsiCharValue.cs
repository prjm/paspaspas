using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     ANSI char value
    /// </summary>
    public class AnsiCharValue : CharValueBase {

        /// <summary>
        ///     crate a new char value
        /// </summary>
        /// <param name="charValue"></param>
        /// <param name="typeId">type id</param>
        public AnsiCharValue(int typeId, byte charValue) : base(typeId)
            => Value = charValue;

        /// <summary>
        ///     convert this value to a a wide char
        /// </summary>
        public override char AsWideChar
            => (char)Value;

        /// <summary>
        ///     fixed type kind <see cref="CommonTypeKind.AnsiCharType"/>
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.AnsiCharType;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => Value;

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public override IOldTypeReference GetOrdinalValue(ITypeRegistry types)
            => types.Runtime.Integers.ToScaledIntegerValue(Value);

        /// <summary>
        ///     char value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override IOldTypeReference CharAt(int index) {
            if (index < 0 || index >= 1)
                return new SpecialValue(SpecialConstantKind.InvalidChar);

            return this;
        }

        /// <summary>
        ///     convert this value to a string value
        /// </summary>
        /// <returns></returns>
        public override string InternalTypeFormat
            => StringUtils.Invariant($@"{new string((char)Value, 1)}");

        /// <summary>
        ///     char value
        /// </summary>
        public byte Value { get; }
    }
}
