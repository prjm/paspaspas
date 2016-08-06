using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     a list of user attributes
    /// </summary>
    public class UserAttributes : ComposedPart<SyntaxPartBase> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public UserAttributes(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format attributes
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            FlattenToPascal(result, x => result.NewLine(), x => x.Punct("["), x => x.Punct("]"));
            if (Count > 0)
                result.NewLine();
        }
    }
}