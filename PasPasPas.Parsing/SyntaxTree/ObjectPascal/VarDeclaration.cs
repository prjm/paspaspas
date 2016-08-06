using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     variable declaration
    /// </summary>
    public class VarDeclaration : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public VarDeclaration(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; internal set; }

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hints { get; internal set; }

        /// <summary>
        ///     var names
        /// </summary>
        public IdentList Identifiers { get; internal set; }

        /// <summary>
        ///     var types
        /// </summary>
        public TypeSpecification TypeDeclaration { get; internal set; }

        /// <summary>
        ///     var values
        /// </summary>
        public VarValueSpecification ValueSpecification { get; internal set; }

        /// <summary>
        ///     format variable declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(Attributes);
            result.NewLine();
            result.Part(Identifiers);
            result.Space();
            result.Punct(":");
            result.Space();
            result.Part(TypeDeclaration);
            result.Space();
            result.Part(ValueSpecification);
            result.Space();
            result.Part(Hints);
            result.Punct(";");
        }
    }
}