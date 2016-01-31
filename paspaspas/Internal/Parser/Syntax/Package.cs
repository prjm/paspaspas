using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     package definition
    /// </summary>
    public class Package : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public Package(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     contans clause
        /// </summary>
        public PackageContains ContainsClause { get; internal set; }

        /// <summary>
        ///     package head
        /// </summary>
        public PackageHead PackageHead { get; internal set; }

        /// <summary>
        ///     requires clause
        /// </summary>
        public PackageRequires RequiresClause { get; internal set; }

        /// <summary>
        ///     format package
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(PackageHead);
            result.Part(RequiresClause);
            result.Part(ContainsClause);
            result.Keyword("end").Space();
            result.Punct(".");
        }
    }
}
