using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

#if DESKTOP
using PasPasPas.Desktop.BackwardCompatibility;
#endif

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     base class for strings
    /// </summary>
    internal abstract class StringValueBase : RuntimeValueBase, IStringValue {

        /// <summary>
        ///     create a new string value
        /// </summary>
        /// <param name="typeDef"></param>
        /// <param name="kind"></param>
        protected StringValueBase(ITypeDefinition typeDef, StringTypeKind kind) : base(typeDef) {
            if (typeDef.BaseType != BaseType.String)
                throw new ArgumentException(string.Empty, nameof(typeDef));

            if (!(typeDef is IStringType stringType))
                throw new ArgumentException(string.Empty, nameof(typeDef));

            if (stringType.Kind != kind)
                throw new ArgumentException(string.Empty, nameof(typeDef));
        }

        /// <summary>
        ///     get the boolean value
        /// </summary>
        public abstract string AsUnicodeString { get; }

        /// <summary>
        ///     number of characters
        /// </summary>
        public abstract int NumberOfCharElements { get; }

        internal IValue Concat(IStringValue string1, IStringValue string2) {
            var typeDef = TypeDefinition.DefiningUnit.TypeRegistry.SystemUnit.UnicodeStringType;
            return new UnicodeStringValue(typeDef, string.Concat(string1.AsUnicodeString, string2.AsUnicodeString));
        }

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
        ///     get a char at a given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract IValue CharAt(int index);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => AsUnicodeString.GetHashCode(StringComparison.Ordinal);

    }
}
