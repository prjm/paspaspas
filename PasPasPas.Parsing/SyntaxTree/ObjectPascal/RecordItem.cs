using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     record item
    /// </summary>
    public class RecordItem : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public RecordItem(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     class item
        /// </summary>
        public bool Class { get; internal set; }

        /// <summary>
        ///     const section
        /// </summary>
        public ConstSection ConstSection { get; internal set; }

        /// <summary>
        ///     record fields
        /// </summary>
        public RecordFieldList Fields { get; internal set; }

        /// <summary>
        ///     method
        /// </summary>
        public ClassMethod MethodDeclaration { get; internal set; }

        /// <summary>
        ///     property
        /// </summary>
        public ClassProperty PropertyDeclaration { get; internal set; }

        /// <summary>
        ///     type
        /// </summary>
        public TypeSection TypeSection { get; internal set; }

        /// <summary>
        ///     var section
        /// </summary>
        public VarSection VarSection { get; internal set; }

        /// <summary>
        ///     format record item
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Class) result.Keyword("class").Space();
            result.Part(Fields);
            result.Part(MethodDeclaration);
            result.Part(PropertyDeclaration);
            result.Part(TypeSection);
            result.Part(VarSection);
            result.Part(ConstSection);
        }
    }
}