using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     create a new for statement
    /// </summary>
    public class ForStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     iteration end
        /// </summary>
        public Expression EndExpression { get; set; }

        /// <summary>
        ///     iteration kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     iteration start
        /// </summary>
        public Expression StartExpression { get; set; }

        /// <summary>
        ///     iteration statement
        /// </summary>
        public Statement Statement { get; set; }

        /// <summary>
        ///     iteration variable
        /// </summary>
        public Identifier Variable { get; set; }

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