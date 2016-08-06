using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     variable value
    /// </summary>
    public class VarValueSpecification : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public VarValueSpecification(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     absolute index
        /// </summary>
        public ConstantExpression Absolute { get; internal set; }

        /// <summary>
        ///     initial value
        /// </summary>
        public ConstantExpression InitialValue { get; internal set; }

        /// <summary>
        ///     format variable values
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Absolute != null) {
                result.Keyword("absolute");
                result.Space();
                result.Part(Absolute);
                return;
            }

            result.Punct("=");
            result.Space();
            result.Part(InitialValue);
        }
    }
}