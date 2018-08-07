using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     if statement
    /// </summary>
    public class IfStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     condition
        /// </summary>
        public ExpressionSymbol Condition { get; set; }

        /// <summary>
        ///     else part
        /// </summary>
        public Statement ElsePart { get; set; }

        /// <summary>
        ///     then part
        /// </summary>
        public Statement ThenPart { get; set; }

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