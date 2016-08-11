using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     set section part
    /// </summary>
    public class SetSectnPart : SyntaxPartBase {

        /// <summary>
        ///     section part
        /// </summary>
        /// <param name="standardParser"></param>
        public SetSectnPart(IParserInformationProvider standardParser) : base(standardParser) {
        }

        /// <summary>
        ///     continuation
        /// </summary>
        public int Continuation { get; internal set; }

        /// <summary>
        ///     set expression
        /// </summary>
        public Expression SetExpression { get; internal set; }

        /// <summary>
        ///     format set expression part
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Continuation) {
                case TokenKind.DotDot:
                    result.Punct("..");
                    break;
                case TokenKind.Comma:
                    result.Punct(",").Space();
                    break;
            }
            result.Part(SetExpression);
        }
    }
}