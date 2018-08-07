using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     goto statement
    /// </summary>
    public class GoToStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     break statement
        /// </summary>
        public bool Break { get; set; }

        /// <summary>
        ///     continue statement
        /// </summary>
        public bool Continue { get; set; }

        /// <summary>
        ///     exit statement
        /// </summary>
        public bool Exit { get; set; }

        /// <summary>
        ///     exit expression
        /// </summary>
        public ExpressionSymbol ExitExpression { get; set; }

        /// <summary>
        ///     goto label
        /// </summary>
        public Label GoToLabel { get; set; }

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