using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface item
    /// </summary>
    public class InterfaceItem : StandardSyntaxTreeBase {

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethod Method { get; set; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public ClassProperty Property { get; set; }

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