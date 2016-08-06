using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     interface part of a unit
    /// </summary>
    public class UnitInterface : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public UnitInterface(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     interface declaration
        /// </summary>
        public InterfaceDeclaration InterfaceDeclaration { get; internal set; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public UsesClause UsesClause { get; internal set; }

        /// <summary>
        ///     format interface declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("interface").NewLine();
            result.Part(UsesClause);
            result.NewLine();
            result.Part(InterfaceDeclaration);
        }
    }
}