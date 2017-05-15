using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     except handler
    /// </summary>
    public class ExceptHandler : StandardSyntaxTreeBase {

        /// <summary>
        ///     handler type
        /// </summary>
        public TypeName HandlerType { get; set; }

        /// <summary>
        ///     handler name
        /// </summary>
        public Identifier Name { get; set; }

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