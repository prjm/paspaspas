using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     external specifier
    /// </summary>
    public class ExternalSpecifier : StandardSyntaxTreeBase {

        /// <summary>
        ///     external expression
        /// </summary>
        public ConstantExpression Expression { get; set; }

        /// <summary>
        ///     external specifier kind
        /// </summary>
        public int Kind { get; set; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }



    }
}