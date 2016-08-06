using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     Quoted string
    /// </summary>
    public class QuotedString : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public QuotedString(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     quoted value
        /// </summary>
        public string QuotedValue
            => UnquotedValue;

        /// <summary>
        ///     unquoted value
        /// </summary>
        public string UnquotedValue { get; internal set; }
            = string.Empty;

        /// <summary>
        ///     format quoated string
        /// </summary>
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result) {
            result.Literal(QuotedValue);
        }
    }
}