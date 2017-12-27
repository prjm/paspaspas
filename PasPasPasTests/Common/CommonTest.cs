using PasPasPas.Api;
using PasPasPas.Infrastructure.Common;
using PasPasPas.Options.DataTypes;
using PasPasPas.Runtime.Operators;
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
            => new RuntimeValues().ToIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(byte number)
            => new RuntimeValues().ToIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(short number)
            => new RuntimeValues().ToIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(ushort number)
            => new RuntimeValues().ToIntegerValue(number);



        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(int number)
            => new RuntimeValues().ToIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(uint number)
            => new RuntimeValues().ToIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(long number)
            => new RuntimeValues().ToIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected IValue GetIntegerValue(ulong number)
            => new RuntimeValues().ToIntegerValue(number);

    }
}
