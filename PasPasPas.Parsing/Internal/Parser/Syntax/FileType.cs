using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     file type
    /// </summary>
    public class FileType : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public FileType(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     packed file
        /// </summary>
        public bool Packed { get; internal set; }

        /// <summary>
        ///     optional subtype
        /// </summary>
        public TypeSpecification TypeDefinition { get; internal set; }

        /// <summary>
        ///     format type definition
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Packed) {
                result.Keyword("packed");
                result.Space();
            }
            result.Keyword("file");
            if (TypeDefinition != null) {
                result.Space();
                result.Keyword("of");
                result.Space();
                TypeDefinition.ToFormatter(result);
            }
        }
    }
}