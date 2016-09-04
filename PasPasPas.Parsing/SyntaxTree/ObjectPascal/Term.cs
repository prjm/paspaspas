using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     term
    /// </summary>
    public class Term : SyntaxPartBase {

        /// <summary>
        ///     create new term
        /// </summary>
        /// <param name="informationProvider"></param>
        public Term(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     term kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public Factor LeftOperand { get; internal set; }

        /// <summary>
        ///     rihgt operand
        /// </summary>
        public Term RightOperand { get; internal set; }

        /// <summary>
        ///     format term
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(LeftOperand);
            if (RightOperand != null) {
                result.Space();
                switch (Kind) {
                    case TokenKind.Times:
                        result.Punct("*");
                        break;
                    case TokenKind.Slash:
                        result.Punct("/");
                        break;
                    case TokenKind.Div:
                        result.Keyword("div");
                        break;
                    case TokenKind.Mod:
                        result.Keyword("mod");
                        break;
                    case TokenKind.And:
                        result.Keyword("and");
                        break;
                    case TokenKind.Shl:
                        result.Keyword("shl");
                        break;
                    case TokenKind.Shr:
                        result.Keyword("shr");
                        break;
                    case TokenKind.As:
                        result.Keyword("as");
                        break;
                }
                result.Space();
                result.Part(RightOperand);
            }
        }
    }
}