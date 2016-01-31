using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     pascal unit
    /// </summary>
    public class Unit : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public Unit(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     unit block
        /// </summary>
        public UnitBlock UnitBlock { get; internal set; }

        /// <summary>
        ///     unit head section
        /// </summary>
        public UnitHead UnitHead { get; internal set; }

        /// <summary>
        ///     unit implementation section
        /// </summary>
        public UnitImplementation UnitImplementation { get; internal set; }

        /// <summary>
        ///     unit interface
        /// </summary>
        public UnitInterface UnitInterface { get; internal set; }

        /// <summary>
        ///     format unit
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(UnitHead);
            result.Part(UnitInterface);
            result.Part(UnitImplementation);
            result.Part(UnitBlock).NewLine();
            result.Punct(".");
        }
    }
}
