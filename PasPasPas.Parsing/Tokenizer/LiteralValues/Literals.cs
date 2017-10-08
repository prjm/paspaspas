using System;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     helper to parse literals
    /// </summary>
    public static class Literals {

        /// <summary>
        ///     parse an integer literal
        /// </summary>
        /// <param name="value">integer literal</param>
        /// <returns>parsed literal</returns>
        public static object ParseIntegerLiteral(StaticEnvironment environment, string value) {
            var parser = environment.Require<IIntegerParser>(StaticDependency.ParsedIntegers);
            return parser.ParseInt(value);
        }

        /// <summary>
        ///     parse an hex number literal
        /// </summary>
        /// <param name="value">hex number literal</param>
        /// <returns>parsed literal</returns>
        public static object ParseHexNumberLiteral(StaticEnvironment environment, string value) {
            var parser = environment.Require<IHexNumberParser>(StaticDependency.ParsedHexNumbers);
            return parser.ParseHexNumber(value);
        }

        /// <summary>
        ///     convert an integer literal value to a cjar
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ConvertCharLiteral(StaticEnvironment environment, object value) {
            var converter = environment.Require<ICharLiteralConverter>(StaticDependency.ConvertedCharLiterals);
            return converter.Convert(value);
        }

        /// <summary>
        ///     test if a parsed literal was a number
        /// </summary>
        /// <param name="parsedValue"></param>
        /// <returns></returns>
        public static bool IsValidInteger(object parsedValue)
            => parsedValue.IsNumber();

        /// <summary>
        ///     create a floating point literal value by the definind parts digits, decimals and exponent
        /// </summary>
        /// <param name="digits">digits</param>
        /// <param name="decimals">decimals</param>
        /// <param name="exponent">exponent</param>
        /// <returns></returns>
        public static object ConvertRealLiteral(StaticEnvironment environment, object digits, object decimals, bool minus, object exponent) {
            var converter = environment.Require<IRealConverter>(StaticDependency.ConvertedRealLiterals);
            return converter.Convert(digits, decimals, minus, exponent);

        }

        /// <summary>
        ///     parser an integer or hex numeric literal
        /// </summary>
        /// <param name="value">value to parse</param>
        /// <param name="valueParser">parser id</param>
        /// <returns></returns>
        public static object NumberLiteral(StaticEnvironment environment, string value, int valueParser) {

            if (valueParser == StaticDependency.ParsedHexNumbers)
                return ParseHexNumberLiteral(environment, value);

            if (valueParser == StaticDependency.ParsedIntegers)
                return ParseIntegerLiteral(environment, value);

            throw new InvalidOperationException();
        }

    }
}
