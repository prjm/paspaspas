using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     library definition
    /// </summary>
    public class Library : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public Library(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     library head
        /// </summary>
        public LibraryHead LibraryHead { get; internal set; }

        /// <summary>
        ///     main block
        /// </summary>
        public Block MainBlock { get; internal set; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public UsesFileClause Uses { get; internal set; }

        /// <summary>
        ///     format library
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (LibraryHead != null)
                LibraryHead.ToFormatter(result);

            if (Uses != null && Uses.Files != null && Uses.Files.Count > 0) {
                result.NewLine();
                Uses.ToFormatter(result);
                result.NewLine();
            }

            result.NewLine();
            MainBlock.ToFormatter(result);
            result.NewLine();
            result.Punct(".");
        }
    }
}
