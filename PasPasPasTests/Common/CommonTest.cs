using PasPasPas.Api;
using PasPasPas.Global.Runtime;
using PasPasPas.Options.DataTypes;
using PasPasPas.Runtime.Values;
using PasPasPas.Typings.Common;

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
            => new RuntimeValues().ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(byte number)
            => new RuntimeValues().ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(short number)
            => new RuntimeValues().ToScaledIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(ushort number)
            => new RuntimeValues().ToScaledIntegerValue(number);



        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(int number)
            => new RuntimeValues().ToScaledIntegerValue(number);

        /// <summary>
        ///     get the Unicode string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected IValue GetUnicodeStringValue(string text)
            => new RuntimeValues().ToUnicodeString(text);

        /// <summary>
        ///     get a wide char value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected IValue GetWideCharValue(char value)
            => new RuntimeValues().ToWideCharValue(value);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(uint number)
            => new RuntimeValues().ToScaledIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(long number)
            => new RuntimeValues().ToScaledIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(ulong number)
            => new RuntimeValues().ToScaledIntegerValue(number);

        protected IValue GetExtendedValue(double number)
            => new RuntimeValues().ToExtendedValue(number);

    }
}
