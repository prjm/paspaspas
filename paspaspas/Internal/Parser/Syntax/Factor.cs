using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     factor
    /// </summary>
    public class Factor : SyntaxPartBase {

        /// <summary>
        ///     create new factor
        /// </summary>
        /// <param name="informationProvider"></param>
        public Factor(IParserInformationProvider informationProvider) : base(informationProvider) {
        }

        /// <summary>
        ///     address of operator
        /// </summary>
        public Factor AddressOf { get; internal set; }

        /// <summary>
        ///     designator (inherited)
        /// </summary>
        public DesignatorStatement Designator { get; internal set; }

        /// <summary>
        ///     hex number
        /// </summary>
        public PascalHexNumber HexValue { get; internal set; }

        /// <summary>
        ///     int value
        /// </summary>
        public PascalInteger IntValue { get; internal set; }

        /// <summary>
        ///     <c>false</c> literal
        /// </summary>
        public bool IsFalse { get; internal set; }

        /// <summary>
        ///     <c>nil</c> literal
        /// </summary>
        public bool IsNil { get; internal set; }

        /// <summary>
        ///     <c>true</c> literal
        /// </summary>
        public bool IsTrue { get; internal set; }

        /// <summary>
        ///     minus
        /// </summary>
        public Factor Minus { get; internal set; }

        /// <summary>
        ///     nor
        /// </summary>
        public Factor Not { get; internal set; }

        /// <summary>
        ///     parented expression
        /// </summary>
        public Expression ParenExpression { get; internal set; }

        /// <summary>
        ///     plus
        /// </summary>
        public Factor Plus { get; internal set; }

        /// <summary>
        ///     pointer to
        /// </summary>
        public PascalIdentifier PointerTo { get; internal set; }

        /// <summary>
        ///     real value
        /// </summary>
        public PascalRealNumber RealValue { get; internal set; }

        /// <summary>
        ///     set section
        /// </summary>
        public SetSectn SetSection { get; internal set; }

        /// <summary>
        ///     string factor
        /// </summary>
        public QuotedString StringValue { get; internal set; }

        /// <summary>
        ///     type cast
        /// </summary>
        public Cast TypeCast { get; internal set; }

        /// <summary>
        ///     format factor
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (AddressOf != null) {
                result.Punct("@");
                result.Part(AddressOf);
                return;
            }

            if (Not != null) {
                result.Operator("not");
                result.Space().Part(Not);
                return;
            }

            if (Plus != null) {
                result.Operator("+");
                result.Part(Plus);
                return;
            }

            if (Minus != null) {
                result.Operator("-");
                result.Part(Minus);
                return;
            }

            if (PointerTo != null) {
                result.Operator("^");
                result.Part(PointerTo);
                return;
            }

            if (ParenExpression != null) {
                result.Punct("(").Part(ParenExpression).Punct(")");
                return;
            }

            if (IsTrue) {
                result.Keyword("true");
                return;
            }

            if (IsFalse) {
                result.Keyword("false");
                return;
            }

            if (IsNil) {
                result.Keyword("nil");
                return;
            }

            result.Part(IntValue);
            result.Part(StringValue);
            result.Part(HexValue);
            result.Part(RealValue);
            result.Part(SetSection);
            result.Part(TypeCast);
            result.Part(Designator);
        }
    }
}