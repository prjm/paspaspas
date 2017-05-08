using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     except handler
    /// </summary>
    public class ExceptHandler : StandardSyntaxTreeBase {
        public ExceptHandler(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

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
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }


    }
}