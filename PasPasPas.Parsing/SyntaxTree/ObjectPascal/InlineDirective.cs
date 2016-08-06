using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     inlining directive
    /// </summary>
    public class InlineDirective : SyntaxPartBase {

        /// <summary>
        ///     inline directive
        /// </summary>
        /// <param name="informationProvider"></param>
        public InlineDirective(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     inline or assembler
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     format
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {
                case PascalToken.Inline:
                    result.Keyword("inline");
                    break;
                case PascalToken.Assembler:
                    result.Keyword("assembler");
                    break;
            }
            result.Punct(";");
        }
    }
}
