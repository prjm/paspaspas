using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     variant section
    /// </summary>
    public class RecordVariantSection : ComposedPart<RecordVariant> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public RecordVariantSection(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     name of the variant
        /// </summary>
        public PascalIdentifier Name { get; internal set; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecification TypeDecl { get; internal set; }

        /// <summary>
        ///     format variant section
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("case").Space();
            if (Name != null) {
                result.Part(Name).Space();
                result.Punct(":").Space();
            }
            result.Part(TypeDecl).Space().Keyword("of");
            result.StartIndent();
            result.NewLine();
            FlattenToPascal(result, x => x.NewLine());
            result.EndIndent();
        }
    }
}