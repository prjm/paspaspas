using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     closure expression
    /// </summary>
    public class ClosureExpression : StandardSyntaxTreeBase {

        public ClosureExpression(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        /// block
        /// </summary>
        public Block Block { get; set; }

        /// <summary>
        ///     closue kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     closure parameters
        /// </summary>
        public FormalParameterSection Parameters { get; set; }

        /// <summary>
        ///     closure return type
        /// </summary>
        public TypeSpecification ReturnType { get; set; }

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