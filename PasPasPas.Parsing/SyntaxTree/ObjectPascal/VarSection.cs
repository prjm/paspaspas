using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     var section
    /// </summary>
    public class VarSection : ComposedPart<VarDeclaration> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public VarSection(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     section kind: var or threadvar
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     format var section
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {

            if (Kind == PascalToken.Var) {
                result.Keyword("var");
            }
            else if (Kind == PascalToken.ThreadVar) {
                result.Keyword("threadvar");
            }

            result.StartIndent();
            result.NewLine();
            FlattenToPascal(result, x => x.NewLine());
            result.EndIndent();
        }
    }
}
