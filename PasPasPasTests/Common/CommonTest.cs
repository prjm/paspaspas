using PasPasPas.Api;
using PasPasPas.Global.Runtime;
using PasPasPas.Options.DataTypes;
using PasPasPas.Runtime.Values;
using PasPasPas.Typings.Common;
using SharpFloat.FloatingPoint;

namespace PasPasPasTests.Common {

    public class CommonTest {

        /// <summary>
        ///     get the basic environment
        /// </summary>
        /// <returns></returns>
        protected ITypedEnvironment CreateEnvironment(NativeIntSize intSize = NativeIntSize.Undefined)
            => new DefaultEnvironment(intSize);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(sbyte number)
            => new RuntimeValueFactory().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(byte number)
            => new RuntimeValueFactory().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(short number)
            => new RuntimeValueFactory().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(ushort number)
            => new RuntimeValueFactory().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(int number)
            => new RuntimeValueFactory().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     get the Unicode string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected IValue GetUnicodeStringValue(string text)
            => new RuntimeValueFactory().Strings.ToUnicodeString(text);

        /// <summary>
        ///     get the Unicode char value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected IValue GetWideCharValue(char text)
            => new RuntimeValueFactory().Chars.ToWideCharValue(text);



        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(uint number)
            => new RuntimeValueFactory().Integers.ToScaledIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(long number)
            => new RuntimeValueFactory().Integers.ToScaledIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(ulong number)
            => new RuntimeValueFactory().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     get extended value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetExtendedValue(ExtF80 number)
            => new RuntimeValueFactory().RealNumbers.ToExtendedValue(number);

    }
}
