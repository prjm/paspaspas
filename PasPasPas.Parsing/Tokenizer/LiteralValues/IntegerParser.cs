using System;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     simple integer parser
    /// </summary>
    public sealed class IntegerParser : IIntegerParser, IHexNumberParser, ILookupFunction<string, object> {

        private readonly LookupTable<string, object> data;

        public LookupTable<string, object> Table
            => data;

        LookupTable ILookupFunction.Table
            => data;

        public static object IntegerOverflowInLiteral
            = new object();

        public static object InvalidIntegerLiteral
            = new object();

        /// <summary>
        ///     get the integral value of a char
        /// </summary>
        /// <param name="character">character</param>
        /// <param name="allowHex">if <c>true</c> then hex chars are allowed</param>
        /// <returns>integral value</returns>
        private static byte GetValueOfChar(char character, bool allowHex) {
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

            if (allowHex) {
                switch (character) {
                    case 'A':
                    case 'a':
                        return 10;
                    case 'B':
                    case 'b':
                        return 11;
                    case 'c':
                    case 'C':
                        return 12;
                    case 'd':
                    case 'D':
                        return 13;
                    case 'e':
                    case 'E':
                        return 14;
                    case 'f':
                    case 'F':
                        return 15;
                }
            }

            return 255;
        }

        private static readonly ulong[] hexFactors = {
            1,
            16,
            256,
            4096,
            65536,
            1048576,
            16777216,
            268435456,
            4294967296,
            68719476736,
            1099511627776,
            17592186044416,
            281474976710656,
            4503599627370496,
            72057594037927936,
            1152921504606846976,
        };

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

        private readonly bool allowHex;

        public IntegerParser(bool allowHexNumbers) {
            allowHex = allowHexNumbers;
            data = new LookupTable<string, object>(new Func<string, object>(DoParse), true);
        }

        /// <summary>
        ///     parse a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private object DoParse(string input) {
            ulong result = 0;
            ulong newresult;

            if (input.Length < 1)
                return InvalidIntegerLiteral;

            for (var i = 0; i < input.Length; i++) {
                var value = GetValueOfChar(input[input.Length - 1 - i], allowHex);

                if (i > 19 || (allowHex && i > 16)) {
                    return IntegerOverflowInLiteral;
                }

                if (value == 255) {
                    return InvalidIntegerLiteral;
                }

                if (allowHex)
                    newresult = result + (value * hexFactors[i]);
                else
                    newresult = result + (value * factors[i]);

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
        /// <param name="input">string to parse</param>
        /// <returns>parsed number</returns>
        public object ParseInt(string input)
            => data.GetValue(input ?? throw new ArgumentNullException(nameof(input)));

        /// <summary>
        ///     parse a hex number
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <returns>parsed number</returns>
        public object ParseHexNumber(string input)
            => data.GetValue(input ?? throw new ArgumentNullException(nameof(input)));
    }
}
