using System;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     helper class to unwrap literals
    /// </summary>
    public class LiteralUnwrapper : IEnvironmentItem, ILiteralUnwrapper {

        /// <summary>
        ///     number of items
        /// </summary>
        public int Count
            => -1;

        /// <summary>
        ///     caption
        /// </summary>
        public string Caption
            => "LiteralUnwrapper";


        /// <summary>
        ///     unwrap a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string UnwrapString(object value)
            => value?.ToString();
    }
}
