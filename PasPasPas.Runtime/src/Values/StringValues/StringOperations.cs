using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     string calculator
    /// </summary>
    public class StringOperations : IStringOperations {

        private readonly ITypeReference invalidString
            = new SpecialValue(SpecialConstantKind.InvalidString);

        private readonly ITypeReference emptyString
            = new EmptyStringValue();

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
        public ITypeReference ToUnicodeString(string text)
            => new UnicodeStringValue(text);

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
        public ITypeReference Concat(ITypeReference value1, ITypeReference value2) {
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
        public ITypeReference Equal(ITypeReference left, ITypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.Equal(string1, string2));
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference GreaterThen(ITypeReference left, ITypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.GreaterThen(string1, string2));
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference GreaterThenEqual(ITypeReference left, ITypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.GreaterThenEqual(string1, string2));
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference LessThen(ITypeReference left, ITypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.LessThen(string1, string2));
            else
                return invalidString;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference LessThenOrEqual(ITypeReference left, ITypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.LessThenOrEqual(string1, string2));
            else
                return invalidString;

        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public ITypeReference NotEquals(ITypeReference left, ITypeReference right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.NotEquals(string1, string2));
            else
                return invalidString;
        }

        /// <summary>
        ///     get the empty string value
        /// </summary>
        /// <returns></returns>
        public ITypeReference EmptyString => emptyString;
    }
}
