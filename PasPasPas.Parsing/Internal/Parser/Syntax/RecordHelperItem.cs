using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     record helper item
    /// </summary>
    public class RecordHelperItem : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public RecordHelperItem(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     method
        /// </summary>
        public ClassMethod MethodDeclaration { get; internal set; }

        /// <summary>
        ///     property
        /// </summary>
        public ClassProperty PropertyDeclaration { get; internal set; }

        /// <summary>
        ///     format record helper item
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(MethodDeclaration);
            result.Part(PropertyDeclaration);
        }
    }
}