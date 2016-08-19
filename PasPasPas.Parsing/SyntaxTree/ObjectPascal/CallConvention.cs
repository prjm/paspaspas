using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     call convention
    /// </summary>
    public class CallConvention : SyntaxPartBase {

        /// <summary>
        ///     create new call convention
        /// </summary>
        /// <param name="informationProvider"></param>
        public CallConvention(IParserInformationProvider informationProvider) : base(informationProvider) {

        }

        /// <summary>
        ///     call convention kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     format call convention
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case TokenKind.Cdecl:
                    result.Keyword("cdecl");
                    break;

                case TokenKind.Pascal:
                    result.Keyword("pascal");
                    break;

                case TokenKind.Register:
                    result.Keyword("register");
                    break;

                case TokenKind.Safecall:
                    result.Keyword("safecall");
                    break;

                case TokenKind.Stdcall:
                    result.Keyword("stdcall");
                    break;

                case TokenKind.Export:
                    result.Keyword("export");
                    break;
            }
            result.Punct(";");
        }
    }
}
