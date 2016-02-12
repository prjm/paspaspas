using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     with statement
    /// </summary>
    public class WithStatement : ComposedPart<DesignatorStatement> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="parser"></param>
        public WithStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     statement
        /// </summary>
        public Statement Statement { get; internal set; }

        /// <summary>
        ///     format with statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("with").Space();
            FlattenToPascal(result, x => x.Punct(",").Space());
            result.Space();
            result.Keyword("do");
            result.StartIndent();
            result.NewLine();
            result.Part(Statement);
            result.EndIndent();
            result.NewLine();
        }
    }
}