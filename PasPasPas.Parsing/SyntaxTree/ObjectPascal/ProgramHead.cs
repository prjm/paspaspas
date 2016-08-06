using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     program header
    /// </summary>
    public class ProgramHead : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ProgramHead(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     name of the program
        /// </summary>
        public NamespaceName Name { get; internal set; }

        /// <summary>
        ///     program parameters
        /// </summary>
        public ProgramParameterList Params { get; internal set; }

        /// <summary>
        ///     format program head
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("program").Space();
            Name.ToFormatter(result);
            Params.ToFormatter(result);
            result.Punct(";");
            result.NewLine();
        }
    }
}