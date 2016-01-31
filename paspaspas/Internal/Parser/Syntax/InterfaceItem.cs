using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     interface item
    /// </summary>
    public class InterfaceItem : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public InterfaceItem(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethod Method { get; internal set; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public ClassProperty Property { get; internal set; }

        /// <summary>
        ///     format interface item
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(Method);
            result.Part(Property);
        }
    }
}