using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     except handler
    /// </summary>
    public class ExceptHandler : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax element
        /// </summary>
        /// <param name="parser"></param>
        public ExceptHandler(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     handler type
        /// </summary>
        public NamespaceName HandlerType { get; internal set; }

        /// <summary>
        ///     handler name
        /// </summary>
        public PascalIdentifier Name { get; internal set; }

        /// <summary>
        ///     statement
        /// </summary>
        public Statement Statement { get; internal set; }

        /// <summary>
        ///     format hander
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("on").Space();
            result.Part(Name).Space();
            result.Punct(":").Space();
            result.Part(HandlerType).Space();
            result.Keyword("do").StartIndent();
            result.NewLine();
            result.Part(Statement);
            result.Punct(";");
            result.EndIndent();
            result.NewLine();
        }
    }
}