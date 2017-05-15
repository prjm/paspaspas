using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {


    /// <summary>
    ///     abstract directive
    /// </summary>
    public class AbstractDirective : StandardSyntaxTreeBase {

        /// <summary>
        ///     final or abstract
        /// </summary>
        public int Kind { get; set; }

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
