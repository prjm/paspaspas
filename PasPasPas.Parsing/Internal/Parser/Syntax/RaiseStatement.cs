using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     raise statemenmt
    /// </summary>
    public class RaiseStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new raise statement
        /// </summary>
        /// <param name="parser"></param>
        public RaiseStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     at part
        /// </summary>
        public DesignatorStatement At { get; internal set; }

        /// <summary>
        ///     raise part
        /// </summary>
        public DesignatorStatement Raise { get; internal set; }

        /// <summary>
        ///     format raise statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("raise");
            if (Raise != null) {
                result.Space().Part(Raise);
            }
            if (At != null) {
                result.Space().Keyword("at").Space().Part(At);
            }
        }
    }
}