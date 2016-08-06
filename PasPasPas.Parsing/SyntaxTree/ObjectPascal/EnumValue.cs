using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     enumeration value
    /// </summary>
    public class EnumValue : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public EnumValue(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     name
        /// </summary>
        public PascalIdentifier EnumName { get; internal set; }

        /// <summary>
        ///     value
        /// </summary>
        public Expression Value { get; internal set; }

        /// <summary>
        ///     format enum
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Space();
            result.Part(EnumName);
            result.Space();
            if (Value != null) {
                result.Operator("=");
                result.Space();
                result.Part(Value);
                result.Space();
            }
        }
    }
}