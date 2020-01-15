using System;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     simple integer parser
    /// </summary>
    public sealed class IntegerParser : IILiteralParser, ILookupFunction<string, IOldTypeReference> {

        private readonly LookupTable<string, IOldTypeReference> data;

        /// <summary>
        ///     lookup table
        /// </summary>
        public LookupTable<string, IOldTypeReference> Table
            => data;

        LookupTable ILookupFunction.Table
            => data;

        /// <summary>
        ///     count entries
        /// </summary>
        public int Count
            => Table.Count;

        /// <summary>
        ///     caption
        /// </summary>
        public string Caption
            => allowHex ? "HexParser" : "IntParser";

        /// <summary>
        ///     overflowed integer literal
        /// </summary>
        public static object IntegerOverflowInLiteral { get; }
            = new object();

        /// <summary>
        ///     invalid integer literal
        /// </summary>
        public static object InvalidIntegerLiteral { get; }
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
        private readonly IRuntimeValueFactory constants;

        /// <summary>
        ///     create a new integer parser
        /// </summary>
        /// <param name="constOperations"></param>
        /// <param name="hexFormat">if <c>true</c>, numbers a parsed in hex format</param>
        public IntegerParser(IRuntimeValueFactory constOperations, bool hexFormat) {
            allowHex = hexFormat;
            constants = constOperations;
            data = new LookupTable<string, IOldTypeReference>(new Func<string, IOldTypeReference>(DoParse), true);
        }

        /// <summary>
        ///     parse a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private IOldTypeReference DoParse(string input) {
            ulong result = 0;
            ulong newresult;

            if (input.Length < 1)
                return constants.Integers.Invalid;

            for (var i = 0; i < input.Length; i++) {
                var value = GetValueOfChar(input[input.Length - 1 - i], allowHex);

                if (i > 19 || allowHex && i >= 16) {
                    return constants.Integers.Overflow;
                }

                if (value == 255) {
                    return constants.Integers.Invalid;
                }

                if (allowHex)
                    newresult = result + value * hexFactors[i];
                else
                    newresult = result + value * factors[i];

                if (newresult < result)
                    return constants.Integers.Overflow;

                result = newresult;
            }

            return constants.Integers.ToScaledIntegerValue(result);
        }

        /// <summary>
        ///     parse an input
        /// </summary>
        /// <param name="input">string to parse</param>
        /// <returns>parsed number</returns>
        public IOldTypeReference Parse(string input)
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
