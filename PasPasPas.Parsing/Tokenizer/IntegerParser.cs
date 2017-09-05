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
    public sealed class IntegerParser : IIntegerParser, ILookupFunction<string, object> {

        private LookupTable<string, object> data
            = new LookupTable<string, object>(new Func<string, object>(DoParse), true);

        public LookupTable<string, object> Table
            => data;

        LookupTable ILookupFunction.Table
            => data;

        public static object IntegerOverflowInLiteral = new object();

        public static object InvalidIntegerLiteral = new object();

        private static byte GetValueOfChar(char character) {
            switch (character) {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
            }
            return 255;
        }

        private static readonly ulong[] factors = {
            1,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000,
            10000000000,
            100000000000,
            1000000000000,
            10000000000000,
            100000000000000,
            1000000000000000,
            10000000000000000,
            100000000000000000,
            1000000000000000000,
            10000000000000000000
        };

        /// <summary>
        ///     parse a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static object DoParse(string input) {
            ulong result = 0;

            for (var i = 0; i < input.Length; i++) {
                var value = GetValueOfChar(input[input.Length - 1 - i]);

                if (i > 19) {
                    return IntegerOverflowInLiteral;
                }

                if (value == 255) {
                    return InvalidIntegerLiteral;
                }

                var newresult = result + (value * factors[i]);
                if (newresult < result) {
                    return IntegerOverflowInLiteral;
                }

                result = newresult;
            }

            if (result < 256)
                return (byte)result;
            else if (result < 65536)
                return (ushort)result;
            else if (result < 4294967296)
                return (uint)result;

            return result;
        }

        /// <summary>
        ///     parse an input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public object Parse(string input)
            => data.GetValue(input);
    }
}
