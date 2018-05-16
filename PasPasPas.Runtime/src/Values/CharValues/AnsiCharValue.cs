using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     ANSI char value
    /// </summary>
    public class AnsiCharValue : CharValueBase {

        private readonly byte value;

        /// <summary>
        ///     crate a new char value
        /// </summary>
        /// <param name="charValue"></param>
        public AnsiCharValue(byte charValue)
            => value = charValue;

        /// <summary>
        ///     type id
        /// </summary>
        /// <see cref="KnownTypeIds.AnsiCharType"/>
        public override int TypeId
            => KnownTypeIds.AnsiCharType;

        /// <summary>
        ///     convert this value to a a wide char
        /// </summary>
        public override char AsWideChar
            => (char)value;

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
            => value;

        /// <summary>
        ///     convert this value to a string value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => StringUtils.Invariant($@"{new string((char)value, 1)}");
    }
}
