using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     user defined attribute (rtti)
    /// </summary>
    public class UserAttribute : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public UserAttribute(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     üaraparameter expressions
        /// </summary>
        public ExpressionList Expressions { get; internal set; }

        /// <summary>
        ///     name of the attribute
        /// </summary>
        public NamespaceName Name { get; internal set; }

        /// <summary>
        ///     format attribute
        /// </summary>
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result) {
            Name.ToFormatter(result);

            if ((Expressions != null) && (Expressions.Parts.Count > 0)) {
                result.Punct("(");
                Expressions.ToFormatter(result);
                result.Punct(")");
            }

        }
    }
}