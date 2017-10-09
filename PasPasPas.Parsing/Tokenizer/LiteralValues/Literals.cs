using System;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     helper to parse literals
    /// </summary>
    public static class Literals {




        /// <summary>
        ///     test if a parsed literal was a number
        /// </summary>
        /// <param name="parsedValue"></param>
        /// <returns></returns>
        public static bool IsValidInteger(object parsedValue)
            => parsedValue.IsNumber();


    }
}
