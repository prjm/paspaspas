using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     object declaration
    /// </summary>
    public class ObjectDeclaration : StandardSyntaxTreeBase {

        /// <summary>
        ///     parent class
        /// </summary>
        public ParentClass ClassParent { get; set; }

        /// <summary>
        ///     object items
        /// </summary>
        public ObjectItems Items { get; set; }

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