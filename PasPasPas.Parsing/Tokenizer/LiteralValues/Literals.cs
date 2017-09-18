using System;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     helper to parse literals
    /// </summary>
    public static class Literals {

        public static readonly Guid ParsedIntegers =
            new Guid(new byte[] { 0x85, 0x19, 0x39, 0xd3, 0x70, 0xa7, 0x98, 0x47, 0xa1, 0x83, 0x8b, 0xdf, 0xd5, 0x48, 0x2a, 0x90 });
        /* {d3391985-a770-4798-a183-8bdfd5482a90} */

        public static readonly Guid ParsedHexNumbers =
            new Guid(new byte[] { 0x5b, 0x5, 0x2b, 0xc4, 0xa0, 0xd2, 0x34, 0x46, 0x9d, 0x85, 0x67, 0xc, 0x9f, 0x60, 0xfc, 0x22 });
        /* {c42b055b-d2a0-4634-9d85-670c9f60fc22} */


        /// <summary>
        ///     parse an integer literal
        /// </summary>
        /// <param name="value">integer literal</param>
        /// <returns>parsed literal</returns>
        public static object ParseIntegerLiteral(string value) {
            var parser = StaticEnvironment.Require<IIntegerParser>(ParsedIntegers);
            return parser.ParseInt(value);
        }

        /// <summary>
        ///     parse an hex number literal
        /// </summary>
        /// <param name="value">hex number literal</param>
        /// <returns>parsed literal</returns>
        public static object ParseHexNumberLiteral(string value) {
            var parser = StaticEnvironment.Require<IHexNumberParser>(ParsedHexNumbers);
            return parser.ParseHexNumber(value);
        }

        /// <summary>
        ///     parser an integer or hex numeric literal
        /// </summary>
        /// <param name="value">value to parse</param>
        /// <param name="valueParser">parser id</param>
        /// <returns></returns>
        public static object NumberLiteral(string value, Guid valueParser) {

            if (valueParser == ParsedHexNumbers)
                return ParseHexNumberLiteral(value);

            if (valueParser == ParsedIntegers)
                return ParseIntegerLiteral(value);

            ExceptionHelper.InvalidOperation();
            return null;
        }
    }
}
