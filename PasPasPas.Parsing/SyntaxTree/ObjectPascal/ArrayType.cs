using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayType : ComposedPart<ArrayIndex> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ArrayType(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     true if the array is of type <c>array of const</c>
        /// </summary>
        public bool ArrayOfConst { get; internal set; }

        /// <summary>
        ///     array type specification
        /// </summary>
        public TypeSpecification TypeSpecification { get; internal set; }

        /// <summary>
        ///     format array type specification
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("array");
            result.Space();

            if (Count > 0) {
                result.Punct("[");
                FlattenToPascal(result, x => x.Punct(","));
                result.Punct("]");
                result.Space();
            }

            result.Keyword("of");
            result.Space();

            if (ArrayOfConst) {
                result.Keyword("const");
            }
            else {
                result.Part(TypeSpecification);
            }
        }
    }
}