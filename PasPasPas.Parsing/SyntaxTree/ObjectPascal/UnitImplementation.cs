using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     unit implementation part
    /// </summary>
    public class UnitImplementation : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public UnitImplementation(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     declaration section
        /// </summary>
        public Declarations DeclarationSections { get; internal set; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public UsesClause UsesClause { get; internal set; }

        /// <summary>
        ///     format implementation par
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("implementation").NewLine();
            result.Part(UsesClause).NewLine();
            result.Part(DeclarationSections);
        }
    }
}