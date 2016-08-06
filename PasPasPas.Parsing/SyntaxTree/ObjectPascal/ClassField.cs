using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     simple field declaration
    /// </summary>
    public class ClassField : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassField(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hint { get; internal set; }

        /// <summary>
        ///     names
        /// </summary>
        public IdentList Names { get; internal set; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDecl { get; internal set; }

        /// <summary>
        ///     format field declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(Names);
            result.Punct(":").Space();
            result.Part(TypeDecl).Space();
            result.Part(Hint).Space();
            result.Punct(";").NewLine();
        }
    }
}