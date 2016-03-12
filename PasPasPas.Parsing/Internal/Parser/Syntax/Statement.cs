using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     single statement
    /// </summary>
    public class Statement : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax element
        /// </summary>
        /// <param name="parser"></param>
        public Statement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     label
        /// </summary>
        public Label Label { get; internal set; }

        /// <summary>
        ///     statement part
        /// </summary>
        public StatementPart Part { get; internal set; }

        /// <summary>
        ///     format statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Label != null) {
                result.Part(Label);
                result.Punct(":").Space();
            }
            result.Part(Part);
        }
    }
}