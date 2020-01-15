using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     string operations
    /// </summary>
    public class StringOperations : IStringOperations {

        private readonly IOldTypeReference invalidString
            = new SpecialValue(SpecialConstantKind.InvalidString);

        /// <summary>
        ///     create a new string operations helper
        /// </summary>
        /// <param name="booleans"></param>
        public StringOperations(IBooleanOperations booleans)
            => Booleans = booleans;

        /// <summary>
        ///     get a constant text value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IOldTypeReference ToUnicodeString(string text)
            => new UnicodeStringValue(KnownTypeIds.UnicodeStringType, text);

        /// <summary>
        ///     boolean operations
        /// </summary>
        public IBooleanOperations Booleans { get; }

        /// <summary>
        ///     concatenate two strings
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public IOldTypeReference Concat(IOldTypeReference value1, IOldTypeReference value2) {
            if (value1 is IStringValue string1 && value2 is IStringValue string2) {

                if (value2 is EmptyStringValue)
                    return value1;

                if (value1 is EmptyStringValue)
                    return value2;

                return StringValueBase.Concat(string1, string2);
            }

            return invalidString;
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference Equal(IOldTypeReference left, IOldTypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.Equal(string1, string2), KnownTypeIds.BooleanType);
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference GreaterThen(IOldTypeReference left, IOldTypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.GreaterThen(string1, string2), KnownTypeIds.BooleanType);
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference GreaterThenEqual(IOldTypeReference left, IOldTypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.GreaterThenEqual(string1, string2), KnownTypeIds.BooleanType);
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference LessThen(IOldTypeReference left, IOldTypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.LessThen(string1, string2), KnownTypeIds.BooleanType);
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference LessThenOrEqual(IOldTypeReference left, IOldTypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.LessThenOrEqual(string1, string2), KnownTypeIds.BooleanType);
            else
                return invalidString;

        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IOldTypeReference NotEquals(IOldTypeReference left, IOldTypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.NotEquals(string1, string2), KnownTypeIds.BooleanType);
            else
                return invalidString;
        }

        /// <summary>
        ///     get an ANSI string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IOldTypeReference ToAnsiString(string text)
            => new AnsiStringValue(KnownTypeIds.AnsiStringType, text);

        /// <summary>
        ///     get the short string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IOldTypeReference ToShortString(string text)
            => new ShortStringValue(KnownTypeIds.ShortStringType, text);

        /// <summary>
        ///     get the empty string value
        /// </summary>
        /// <returns></returns>
        public IOldTypeReference EmptyString { get; }
            = new EmptyStringValue(KnownTypeIds.UnicodeStringType);
    }
}
