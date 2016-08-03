using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     assembly attribute
    /// </summary>
    public class AssemblyAttribute : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="standardParser"></param>
        public AssemblyAttribute(IParserInformationProvider standardParser) : base(standardParser) {
        }

        /// <summary>
        ///     attribute definition
        /// </summary>
        public UserAttribute Attribute { get; internal set; }

        /// <summary>
        ///     format attribute
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Punct("[");
            result.Keyword("assembly");
            result.Punct(":");
            result.Part(Attribute);
            result.Punct("]");
        }
    }
}
