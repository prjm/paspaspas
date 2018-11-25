using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.StringValues {

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

        /// <summary>
        ///     convert this value to an internal type format
        /// </summary>
        public abstract string InternalTypeFormat { get; }

        /// <summary>
        ///     convert this value to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

        /// <summary>
        ///     constant value
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.ConstantValue;

        internal static ITypeReference Concat(IStringValue string1, IStringValue string2)
            => new UnicodeStringValue(string.Concat(string1, string2));

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
    }
}