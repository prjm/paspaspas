using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     class helper declaration
    /// </summary>
    public class ClassHelperDef : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassHelperDef(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     items
        /// </summary>
        public ClassHelperItems HelperItems { get; internal set; }

        /// <summary>
        ///     class parent
        /// </summary>
        public ParentClass ClassParent { get; internal set; }

        /// <summary>
        ///     class helper name
        /// </summary>
        public NamespaceName HelperName { get; internal set; }

        /// <summary>
        ///     format class helper
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("class").Space();
            result.Keyword("helper").Space();
            result.Part(ClassParent).Space();
            result.Keyword("for").Space();
            result.Part(HelperName);
            result.StartIndent();
            result.NewLine().Part(HelperItems);
            result.EndIndent();
            result.Keyword("end");
        }
    }
}