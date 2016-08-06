using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     uses clause with file names
    /// </summary>
    public class UsesFileClause : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public UsesFileClause(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     Namespace files
        /// </summary>
        public NamespaceFileNameList Files { get; internal set; }

        /// <summary>
        ///     formats the uses clause as pascal
        /// </summary>
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("uses");
            result.StartIndent();
            result.NewLine();
            result.Part(Files);
            result.EndIndent();
        }
    }
}
