using PasPasPas.Api;
using PasPasPas.Globals.Runtime;
using PasPasPas.Options.DataTypes;
using PasPasPas.Runtime.Values;
using PasPasPas.Runtime.Values.BooleanValues;
using PasPasPas.Runtime.Values.FloatValues;
using PasPasPas.Runtime.Values.IntValues;
using PasPasPas.Typings.Common;
using SharpFloat.FloatingPoint;

namespace PasPasPasTests.Common {

    public class CommonTest {

        /// <summary>
        ///     get the basic environment
        /// </summary>
        /// <returns></returns>
        protected static ITypedEnvironment CreateEnvironment(NativeIntSize intSize = NativeIntSize.Undefined)
            => new DefaultEnvironment(intSize);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(sbyte number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(byte number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(short number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(ushort number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(int number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     get the Unicode string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static ITypeReference GetUnicodeStringValue(string text)
            => new RuntimeValueFactory(null).Strings.ToUnicodeString(text);

        /// <summary>
        ///     get the Unicode char value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static ITypeReference GetWideCharValue(char text)
            => new RuntimeValueFactory(null).Chars.ToWideCharValue(text);



        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(uint number)
            => IntegerValueBase.ToScaledIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(long number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(ulong number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     get extended value
        /// </summary>
        /// <param name="number">number</param>
        /// <returns>number value</returns>
        protected static ITypeReference GetExtendedValue(in ExtF80 number)
            => FloatValueBase.ToExtendedValue(number);

        /// <summary>
        ///     get a boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static ITypeReference GetBooleanValue(bool value)
            => value ?
            new RuntimeValueFactory(null).Booleans.TrueValue :
            new RuntimeValueFactory(null).Booleans.FalseValue;

    }
}
