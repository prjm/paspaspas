using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     designator item
    /// </summary>
    public class DesignatorItem : ComposedPart<FormattedExpression> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="parser"></param>
        public DesignatorItem(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     dereference
        /// </summary>
        public bool Dereference { get; internal set; }

        /// <summary>
        ///     index expression
        /// </summary>
        public ExpressionList IndexExpression { get; internal set; }

        /// <summary>
        ///     subitem
        /// </summary>
        public PascalIdentifier Subitem { get; internal set; }

        /// <summary>
        ///         format designator item
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (Dereference) {
                result.Operator("^");
                return;
            }

            if (Subitem != null) {
                result.Operator(".");
                result.Part(Subitem);
                return;
            }

            if (IndexExpression != null) {
                result.Punct("[");
                result.Part(IndexExpression);
                result.Punct("]");
                return;
            }

            result.Punct("(");
            FlattenToPascal(result, x => x.Punct(",").Space());
            result.Punct(")");
        }
    }
}