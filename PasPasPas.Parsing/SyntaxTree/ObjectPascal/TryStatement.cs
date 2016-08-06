using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     try statement
    /// </summary>
    public class TryStatement : SyntaxPartBase {

        /// <summary>
        ///     create a new try statement
        /// </summary>
        /// <param name="parser"></param>
        public TryStatement(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     finally part
        /// </summary>
        public StatementList Finally { get; internal set; }

        /// <summary>
        ///     except handlers
        /// </summary>
        public ExceptHandlers Handlers { get; internal set; }

        /// <summary>
        ///     try part
        /// </summary>
        public StatementList Try { get; internal set; }

        /// <summary>
        ///     format try statement
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("try").StartIndent();
            result.NewLine().Part(Try).EndIndent();
            result.NewLine();
            if (Finally != null) {
                result.Keyword("finally").StartIndent();
                result.NewLine().Part(Finally).EndIndent();
                result.NewLine();
            }
            if (Handlers != null) {
                result.Keyword("except").StartIndent();
                result.NewLine().Part(Handlers).EndIndent();
                result.NewLine();
            }
            result.Keyword("end").NewLine();
        }
    }
}