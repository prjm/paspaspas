using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     block body
    /// </summary>
    public class BlockBody : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public BlockBody(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///    assembler block
        /// </summary>
        public AsmStatement AssemblerBlock { get; internal set; }

        /// <summary>
        ///     block bode
        /// </summary>
        public CompoundStatement Body { get; internal set; }

        /// <summary>
        ///     format block body
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(AssemblerBlock);
            result.Part(Body);
        }
    }
}