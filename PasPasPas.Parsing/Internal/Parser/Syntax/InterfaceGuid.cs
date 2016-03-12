using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     guid declaration
    /// </summary>
    public class InterfaceGuid : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public InterfaceGuid(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     guid for this interface
        /// </summary>
        public QuotedString Id { get; internal set; }

        /// <summary>
        ///     format guid
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Punct("[");
            result.Part(Id);
            result.Punct("]").NewLine();
        }
    }
}