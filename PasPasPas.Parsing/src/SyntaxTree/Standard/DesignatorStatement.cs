using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     designator
    /// </summary>
    public class DesignatorStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     inherited
        /// </summary>
        public bool Inherited { get; set; }

        /// <summary>
        ///     name
        /// </summary>
        public TypeName Name { get; set; }

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