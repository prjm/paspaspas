using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     record helper
    /// </summary>
    public class RecordHelperDef : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public RecordHelperDef(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     record helper items
        /// </summary>
        public RecordHelperItems Items { get; internal set; }

        /// <summary>
        ///     record helper name
        /// </summary>
        public NamespaceName Name { get; internal set; }

        /// <summary>
        ///     format record helper
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("record").Space();
            result.Keyword("helper").Space();
            result.Keyword("for").Space();
            result.Part(Name);
            result.StartIndent();
            result.NewLine();
            result.Part(Items);
            result.NewLine();
            result.Keyword("end");
            result.EndIndent();
            result.NewLine();
        }
    }
}