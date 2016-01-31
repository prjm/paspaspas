using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     unit initialization part
    /// </summary>
    public class UnitInitialization : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public UnitInitialization(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     unit finalization
        /// </summary>
        public UnitFinalization Finalization { get; internal set; }

        /// <summary>
        ///     format initialization
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("initialization").NewLine();
            if (Finalization != null)
                result.Part(Finalization);
            result.NewLine().Keyword("end");
        }
    }
}