using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     unit head
    /// </summary>
    public class UnitHead : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public UnitHead(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     hinting directives
        /// </summary>
        public HintingInformationList Hint { get; internal set; }

        /// <summary>
        ///     unit name
        /// </summary>
        public NamespaceName UnitName { get; internal set; }

        /// <summary>
        ///     format unit head
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("unit").Space();
            result.Part(UnitName);
            result.Part(Hint).Punct(";").NewLine().NewLine();
        }
    }
}