using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     unit block
    /// </summary>
    public class UnitBlock : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public UnitBlock(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     initializarion
        /// </summary>
        public UnitInitialization Initialization { get; internal set; }

        /// <summary>
        ///     main block
        /// </summary>
        public CompoundStatement MainBlock { get; internal set; }

        /// <summary>
        ///     format block
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (MainBlock == null && Initialization == null) {
                result.Keyword("end");
                return;
            }

            result.Part(MainBlock);
            result.Part(Initialization);
        }
    }
}