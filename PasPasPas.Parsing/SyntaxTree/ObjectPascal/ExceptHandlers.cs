using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     except handlers
    /// </summary>
    public class ExceptHandlers : ComposedPart<ExceptHandler> {

        /// <summary>
        ///     create a new syntax element
        /// </summary>
        /// <param name="parser"></param>
        public ExceptHandlers(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     else statements
        /// </summary>
        public StatementList ElseStatements { get; internal set; }

        /// <summary>
        ///     generic except handler statements
        /// </summary>
        public StatementList Statements { get; internal set; }

        /// <summary>
        ///     format handlers
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Statements != null) {
                result.Part(Statements);
                return;
            }

            FlattenToPascal(result, x => x.NewLine());
            result.NewLine();
            if (ElseStatements != null) {
                result.Keyword("else").StartIndent();
                result.NewLine();
                result.Part(ElseStatements);
                result.EndIndent();
                result.NewLine();
            }
        }
    }
}