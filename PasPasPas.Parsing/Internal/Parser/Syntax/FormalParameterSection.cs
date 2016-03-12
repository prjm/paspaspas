using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     formal parameter section
    /// </summary>
    public class FormalParameterSection : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public FormalParameterSection(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     parameter list
        /// </summary>
        public FormalParameters ParameterList { get; internal set; }

        /// <summary>
        ///     format parameters
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Punct("(");
            result.Part(ParameterList);
            result.Punct(")");
        }
    }
}