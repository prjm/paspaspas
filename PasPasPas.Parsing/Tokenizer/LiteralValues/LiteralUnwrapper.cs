using System;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {
    public class LiteralUnwrapper : ILiteralUnwrapper {

        /// <summary>
        ///     unwrap an hex
        /// </summary>
        /// <param name="parsedValue"></param>
        /// <returns></returns>
        public ulong UnwrapHexnumber(object parsedValue)
            => Convert.ToUInt64(parsedValue);

        /// <summary>
        ///     unwrap an int
        /// </summary>
        /// <param name="parsedValue"></param>
        /// <returns></returns>
        public ulong UnwrapInteger(object parsedValue)
            => Convert.ToUInt64(parsedValue);

        /// <summary>
        ///     unwrap a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string UnwrapString(object value)
            => value?.ToString();
    }
}
