using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///  while statement
    /// </summary>
    public class WhileStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     condition
        /// </summary>
        public Expression Condition { get; set; }

        /// <summary>
        ///     statement
        /// </summary>
        public Statement Statement { get; set; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}