using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     array index definition
    /// </summary>
    public class ArrayIndex : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ArrayIndex(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     start index
        /// </summary>
        public ConstantExpression StartIndex { get; internal set; }

        /// <summary>
        ///     end index
        /// </summary>
        public ConstantExpression EndIndex { get; internal set; }

        /// <summary>
        ///     format 
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(StartIndex);
            if (EndIndex != null) {
                result.Punct(".");
                result.Punct(".");
                result.Part(EndIndex);
            }
        }
    }
}