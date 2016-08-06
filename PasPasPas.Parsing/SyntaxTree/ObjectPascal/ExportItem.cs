using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     exported item
    /// </summary>
    public class ExportItem : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ExportItem(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     index parameter
        /// </summary>
        public Expression IndexParameter { get; internal set; }

        /// <summary>
        ///     name parameter
        /// </summary>
        public Expression NameParameter { get; internal set; }

        /// <summary>
        ///     parameter list
        /// </summary>
        public FormalParameters Parameters { get; internal set; }

        /// <summary>
        ///     resident flag
        /// </summary>
        public bool Resident { get; internal set; }

        /// <summary>
        ///     format exported item
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Parameters != null) {
                result.Punct("(");
                result.Part(Parameters);
                result.Punct(")");
                result.Space();
            }

            if (IndexParameter != null) {
                result.Keyword("index");
                result.Space();
                result.Part(IndexParameter);
                result.Space();
            }

            if (NameParameter != null) {
                result.Keyword("name");
                result.Space();
                result.Part(NameParameter);
                result.Space();
            }

            if (Resident)
                result.Keyword("resident").Space();
        }
    }
}