using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     label declaration
    /// </summary>
    public class Label : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public Label(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     label name
        /// </summary>
        public SyntaxPartBase LabelName { get; internal set; }

        /// <summary>
        ///     format label
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(LabelName);
        }
    }
}