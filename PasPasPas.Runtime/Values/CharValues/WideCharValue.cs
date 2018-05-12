using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.CharValues {

    /// <summary>
    ///     wide char (word) value
    /// </summary>
    public class WideCharValue : CharValueBase, IEquatable<WideCharValue> {

        private readonly char value;

        /// <summary>
        ///     create a new wide char value
        /// </summary>
        /// <param name="character"></param>
        public WideCharValue(char character)
            => value = character;

        /// <summary>
        ///     type id
        /// </summary>
        public override int TypeId
            => KnownTypeIds.WideCharType;

        /// <summary>
        ///     char value
        /// </summary>
        public override char AsWideChar
            => value;

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.WideCharType;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => obj is WideCharValue charValue && charValue.value == value;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(WideCharValue other)
            => value == other.value;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => value.GetHashCode();

        /// <summary>
        ///     convert this value to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => StringUtils.Invariant($"{value}");
    }
}
