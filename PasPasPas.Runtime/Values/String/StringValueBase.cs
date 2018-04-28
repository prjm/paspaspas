using PasPasPas.Global.Runtime;
using PasPasPas.Runtime.Values.Boolean;

namespace PasPasPas.Runtime.Values.String {

    /// <summary>
    ///     base class for strings
    /// </summary>
    public abstract class StringValueBase : IStringValue {

        /// <summary>
        ///     get the type id
        /// </summary>
        public abstract int TypeId { get; }

        /// <summary>
        ///     get the boolean value
        /// </summary>
        public abstract string AsUnicodeString { get; }

        /// <summary>
        ///     always <c>true</c> for string constants
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     type kind
        /// </summary>
        public abstract CommonTypeKind TypeKind { get; }

        internal static IValue Concat(IStringValue string1, IStringValue string2)
            => new UnicodeStringValue(string.Concat(string1, string2));

        internal static IValue Equal(IStringValue string1, IStringValue string2)
            => new BooleanValue(string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) == 0);

        internal static IValue GreaterThen(IStringValue string1, IStringValue string2)
            => new BooleanValue(string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) > 0);

        internal static IValue GreaterThenEqual(IStringValue string1, IStringValue string2)
            => new BooleanValue(string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) >= 0);

        internal static IValue LessThen(IStringValue string1, IStringValue string2)
            => new BooleanValue(string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) < 0);

        internal static IValue LessThenOrEqual(IStringValue string1, IStringValue string2)
            => new BooleanValue(string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) < 0);

        internal static IValue NotEquals(IStringValue string1, IStringValue string2)
            => new BooleanValue(string.CompareOrdinal(string1.AsUnicodeString, string2.AsUnicodeString) != 0);
    }
}