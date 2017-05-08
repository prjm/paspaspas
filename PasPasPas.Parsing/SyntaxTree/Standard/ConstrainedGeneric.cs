using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     generic constraint
    /// </summary>
    public class ConstrainedGeneric : StandardSyntaxTreeBase {
        public ConstrainedGeneric(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     class constraints
        /// </summary>
        public bool ClassConstraint { get; set; }

        /// <summary>
        ///     constraint identifier
        /// </summary>
        public Identifier ConstraintIdentifier { get; set; }

        /// <summary>
        ///     constructor constraint
        /// </summary>
        public bool ConstructorConstraint { get; set; }

        /// <summary>
        ///     record constraint
        /// </summary>
        public bool RecordConstraint { get; set; }

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