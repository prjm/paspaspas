using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     generic constraint
    /// </summary>
    public class ConstrainedGeneric : StandardSyntaxTreeBase {

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
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}