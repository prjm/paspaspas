using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     base class for strings
    /// </summary>
    public abstract class StringValueBase : RuntimeValueBase, IStringValue, IEquatable<IStringValue> {

        /// <summary>
        ///     create a new string value
        /// </summary>
        /// <param name="typeId"></param>
        protected StringValueBase(int typeId) : base(typeId) { }

        /// <summary>
        ///     get the boolean value
        /// </summary>
        public abstract string AsUnicodeString { get; }

        /// <summary>
        ///     convert this value to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

        /// <summary>
        ///     number of characters
        /// </summary>
        public abstract int NumberOfCharElements { get; }

        internal static ITypeReference Concat(IStringValue string1, IStringValue string2)
            => new UnicodeStringValue(KnownTypeIds.UnicodeStringType, string.Concat(string1.AsUnicodeString, string2.AsUnicodeString));

        internal static bool Equal(IStringValue string1, IStringValue string2)
            => string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) == 0;

        internal static bool GreaterThen(IStringValue string1, IStringValue string2)
            => string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) > 0;

        internal static bool GreaterThenEqual(IStringValue string1, IStringValue string2)
            => string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) >= 0;

        internal static bool LessThen(IStringValue string1, IStringValue string2)
            => string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) < 0;

        internal static bool LessThenOrEqual(IStringValue string1, IStringValue string2)
            => string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) < 0;

        internal static bool NotEquals(IStringValue string1, IStringValue string2)
            => string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) != 0;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IStringValue other)
            => Equal(this, other);


        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => obj is IStringValue other ? Equals(other) : false;

        /// <summary>
        ///     get a char at a given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract ITypeReference CharAt(int index);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
#if DESKTOP
            => AsUnicodeString.GetHashCode();
#else
            => AsUnicodeString.GetHashCode(StringComparison.Ordinal);
#endif

    }
}
