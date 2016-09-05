using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     pascal unit
    /// </summary>
    public class Unit : SyntaxPartBase {

        /// <summary>
        ///     unit block
        /// </summary>
        public UnitBlock UnitBlock { get; set; }

        /// <summary>
        ///     unit head section
        /// </summary>
        public UnitHead UnitHead { get; set; }

        /// <summary>
        ///     unit implementation section
        /// </summary>
        public UnitImplementation UnitImplementation { get; set; }

        /// <summary>
        ///     unit interface
        /// </summary>
        public UnitInterface UnitInterface { get; set; }

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
