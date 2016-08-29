using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     string type
    /// </summary>
    public class StringType : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public StringType(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     code page
        /// </summary>
        public ConstantExpression CodePage { get; internal set; }

        /// <summary>
        ///     kind of the string
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     string length
        /// </summary>
        public Expression StringLength { get; internal set; }

        /// <summary>
        ///     format type
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Kind == TokenKind.ShortString) {
                result.Keyword("shortstring").Space();
                return;
            }

            if (Kind == TokenKind.WideString) {
                result.Keyword("widestring").Space();
                return;
            }

            if (Kind == TokenKind.UnicodeString) {
                result.Keyword("unicodestring").Space();
                return;
            }

            if (Kind == TokenKind.AnsiString) {
                result.Keyword("ansistring");
                if (CodePage != null) {
                    result.Punct("(");
                    result.Part(CodePage);
                    result.Punct(")");
                }
                result.Space();
                return;
            }

            if (Kind == TokenKind.String) {
                result.Keyword("string");
                if (StringLength != null) {
                    result.Punct("[");
                    result.Part(StringLength);
                    result.Punct("]");
                }
                result.Space();
                return;
            }
        }
    }
}