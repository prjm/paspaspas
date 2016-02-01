using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     library head
    /// </summary>
    public class LibraryHead : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public LibraryHead(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hints { get; internal set; }

        /// <summary>
        ///     library name
        /// </summary>
        public NamespaceName Name { get; internal set; }

        /// <summary>
        ///     format library
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("library").Space();
            result.Part(Name);
            result.Part(Hints);
            result.Punct(";");
            result.NewLine();
        }
    }
}