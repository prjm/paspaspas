using System;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {
    /// <summary>
    ///     converts a literal integer to a char value
    /// </summary>
    public class CharLiteralConverter : IEnvironmentItem, ICharLiteralConverter, ILookupFunction<object, object> {

        private LookupTable<object, object> data;

        /// <summary>
        ///     invalid character literal
        /// </summary>
        public static readonly object InvalidCharacterLiteral
            = new object();

        public LookupTable<object, object> Table
            => data;

        LookupTable ILookupFunction.Table
            => data;

        public int Count
            => Table.Count;

        public string Caption
            => "CharParser";

        /// <summary>
        ///     create a new char literal converter
        /// </summary>
        public CharLiteralConverter()
            => data = new LookupTable<object, object>(ConvertCharLiteral, true);

        /// <summary>
        ///     convert the literal value
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        public object ConvertCharLiteral(object literal) {
            if (literal == null)
                throw new ArgumentNullException(nameof(literal));

            if (literal.IsNumber())
                return System.Convert.ToChar(literal);
            else
                return InvalidCharacterLiteral;
        }

        /// <summary>
        ///     converts a literal
        /// </summary>
        /// <param name="literal"></param>
        /// <returns></returns>
        public object Convert(object literal)
            => data.GetValue(literal ?? throw new ArgumentNullException(nameof(literal)));
    }
}
