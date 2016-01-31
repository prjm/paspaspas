using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     struct type
    /// </summary>
    public class StructType : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public StructType(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     Packed struct type
        /// </summary>
        public bool Packed { get; internal set; }

        /// <summary>
        ///     part
        /// </summary>
        public StructTypePart Part { get; internal set; }

        /// <summary>
        ///     format type
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Packed)
                result.Keyword("packed").Space();
            result.Part(Part);
        }
    }
}