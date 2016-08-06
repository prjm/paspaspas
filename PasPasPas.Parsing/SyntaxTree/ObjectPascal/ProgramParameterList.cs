using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     list of program parameters
    /// </summary>
    public class ProgramParameterList : ComposedPart<PascalIdentifier> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ProgramParameterList(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format params in pascal syntax
        /// </summary>
        /// <param name="result">result</param>
        public override void ToFormatter(PascalFormatter result) {
            if (Count < 1)
                return;

            result.Punct("(");
            FlattenToPascal(result, x => x.Punct(",").Space());
            result.Punct(")");
        }

    }
}
