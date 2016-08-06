using PasPasPas.Api;
using System.Linq;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     generic definition
    /// </summary>
    public class GenericDefinition : ComposedPart<GenericDefinitionPart> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public GenericDefinition(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format generic definition
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Punct("<");

            if (Items.All(t => t.Count < 1))
                FlattenToPascal(result, x => x.Punct(", "));
            else
                FlattenToPascal(result, x => x.Punct("; "));

            result.Punct(">");
        }
    }
}