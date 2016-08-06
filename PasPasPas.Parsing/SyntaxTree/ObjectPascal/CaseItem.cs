using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     case item
    /// </summary>
    public class CaseItem : ComposedPart<CaseLabel> {

        /// <summary>
        ///     create a new case item
        /// </summary>
        /// <param name="parser"></param>
        public CaseItem(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     case statement
        /// </summary>
        public Statement CaseStatement { get; internal set; }

        /// <summary>
        ///     format case item
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => x.Punct(",").Space());
            result.Punct(":");
            result.StartIndent();
            result.NewLine();
            result.Part(CaseStatement);
            result.EndIndent();
            result.NewLine();
        }
    }
}