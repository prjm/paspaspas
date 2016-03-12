using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

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
                case PascalToken.Cdecl:
                    result.Keyword("cdecl");
                    break;

                case PascalToken.Pascal:
                    result.Keyword("pascal");
                    break;

                case PascalToken.Register:
                    result.Keyword("register");
                    break;

                case PascalToken.Safecall:
                    result.Keyword("safecall");
                    break;

                case PascalToken.Stdcall:
                    result.Keyword("stdcall");
                    break;

                case PascalToken.Export:
                    result.Keyword("export");
                    break;
            }
            result.Punct(";");
        }
    }
}
