using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     simple integer parser
    /// </summary>
    public class IntegerParser : IIntegerParser, ILookupFunction<string, object> {

        private LookupTable<string, object> data
            = new LookupTable<string, object>(new Func<string, object>(DoParse), true);

        public LookupTable<string, object> Table
            => data;

        LookupTable ILookupFunction.Table
            => data;

        /// <summary>
        ///     parse a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static object DoParse(string input)
            => int.Parse(input);

        /// <summary>
        ///     parse an input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public object Parse(string input)
            => data.GetValue(input);
    }
}
