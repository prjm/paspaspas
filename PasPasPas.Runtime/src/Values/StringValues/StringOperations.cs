using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Other;

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     string operations
    /// </summary>
    public class StringOperations : IStringOperations {

        private readonly Lazy<IValue> invalidString;
        private readonly Lazy<IValue> emptyString;

        private readonly ITypeRegistryProvider provider;

        /// <summary>
        ///     create a new boolean operation support class
        /// </summary>
        /// <param name="typeRegistryProvider">type definition provider</param>
        /// <param name="booleans">boolean operations</param>
        public StringOperations(ITypeRegistryProvider typeRegistryProvider, IBooleanOperations booleans) {
            Booleans = booleans;
            provider = typeRegistryProvider;
            invalidString = new Lazy<IValue>(() => new ErrorValue(provider.GetErrorType(), SpecialConstantKind.InvalidString), true);
            emptyString = new Lazy<IValue>(() => new EmptyStringValue(provider.GetShortStringType()));
        }

        /// <summary>
        ///     get a constant text value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IValue ToUnicodeString(string text)
            => new UnicodeStringValue(provider.GetUnicodeStringType(), text);

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
        public IValue Concat(IValue value1, IValue value2) {
            if (value1 is IStringValue string1 && value2 is IStringValue string2) {

                if (value2 is EmptyStringValue)
                    return value1;

                if (value1 is EmptyStringValue)
                    return value2;

                return StringValueBase.Concat(string1, string2);
            }

            return invalidString.Value;
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue Equal(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.Equal(string1, string2));
            else
                return invalidString.Value;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue GreaterThen(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.GreaterThen(string1, string2));
            else
                return invalidString.Value;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue GreaterThenEqual(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.GreaterThenEqual(string1, string2));
            else
                return invalidString.Value;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue LessThen(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.LessThen(string1, string2));
            else
                return invalidString.Value;
        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue LessThenOrEqual(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.LessThenOrEqual(string1, string2));
            else
                return invalidString.Value;

        }

        /// <summary>
        ///     compare values
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public IValue NotEquals(IValue left, IValue right) {
            if (left is IStringValue string1 && right is IStringValue string2)
                return Booleans.ToBoolean(StringValueBase.NotEquals(string1, string2));
            else
                return invalidString.Value;
        }

        /// <summary>
        ///     get an ANSI string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IValue ToAnsiString(string text)
            => new AnsiStringValue(provider.GetAnsiStringType(), text);

        /// <summary>
        ///     get the short string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IValue ToShortString(string text)
            => new ShortStringValue(provider.GetShortStringType(), text);

        /// <summary>
        ///     converts a string value to a wide string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IValue ToWideString(string text)
            => new ShortStringValue(provider.GetWideStringType(), text);

        /// <summary>
        ///     get the empty string value
        /// </summary>
        /// <returns></returns>
        public IValue EmptyString
            => emptyString.Value;

        /// <summary>
        ///     invalid string
        /// </summary>
        public IValue Invalid
            => invalidString.Value;
    }
}
