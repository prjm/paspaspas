using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     file block
    /// </summary>
    public class Block : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public Block(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     block body
        /// </summary>
        public BlockBody Body { get; internal set; }

        /// <summary>
        ///     declarations
        /// </summary>
        public Declarations DeclarationSections { get; internal set; }

        /// <summary>
        ///     format block
        /// </summary>
        /// <param name="result">output</param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(DeclarationSections);
            result.NewLine();
            result.Part(Body);
        }
    }
}