using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     parent class definition
    /// </summary>
    public class ParentClass : ComposedPart<NamespaceName> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ParentClass(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format parent class
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Count < 1)
                return;

            result.Punct("(");
            FlattenToPascal(result, x => x.Punct(", "));
            result.Punct(")");
        }
    }
}