using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     object declaration
    /// </summary>
    public class ObjectDeclaration : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ObjectDeclaration(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     parent class
        /// </summary>
        public ParentClass ClassParent { get; internal set; }

        /// <summary>
        ///     object items
        /// </summary>
        public ObjectItems Items { get; internal set; }

        /// <summary>
        ///     format object declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("object").Space();
            result.Part(ClassParent).Space();
            result.StartIndent();
            result.NewLine();
            result.Part(Items);
            result.Keyword("end");
            result.EndIndent();
        }
    }
}