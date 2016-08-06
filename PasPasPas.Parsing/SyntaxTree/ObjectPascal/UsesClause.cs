using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     uses clause
    /// </summary>
    public class UsesClause : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public UsesClause(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     names of the units to use
        /// </summary>
        public NamespaceNameList UsesList { get; internal set; }

        /// <summary>
        ///     format uses clause
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("uses");
            result.StartIndent();
            result.NewLine();
            result.Part(UsesList);
            result.EndIndent();
            return;
        }
    }
}