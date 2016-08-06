using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     class declaration
    /// </summary>
    public class ClassDeclaration : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassDeclaration(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     sealed class
        /// </summary>
        public bool Abstract { get; internal set; }

        /// <summary>
        ///     items of a class declaration
        /// </summary>
        public ClassDeclarationItems ClassItems { get; internal set; }

        /// <summary>
        ///     parent class
        /// </summary>
        public ParentClass ClassParent { get; internal set; }

        /// <summary>
        ///     abstract class
        /// </summary>
        public bool Sealed { get; internal set; }

        /// <summary>
        ///     format class declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("class");

            if (Sealed) {
                result.Space();
                result.Keyword("sealed");
            }

            if (Abstract) {
                result.Space();
                result.Keyword("abstract");
            }

            ClassParent.ToFormatter(result);

            result.StartIndent();
            result.NewLine();
            ClassItems.ToFormatter(result);
            result.EndIndent();
            result.NewLine();
            result.Keyword("end");
        }
    }
}