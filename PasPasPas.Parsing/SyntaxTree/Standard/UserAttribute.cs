using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     user defined attribute (rtti)
    /// </summary>
    public class UserAttributeDefinition : StandardSyntaxTreeBase {

        /// <summary>
        ///     üaraparameter expressions
        /// </summary>
        public ExpressionList Expressions { get; set; }

        /// <summary>
        ///     name of the attribute
        /// </summary>
        public NamespaceName Name { get; set; }

        /// <summary>
        ///     attribute prefix
        /// </summary>
        public Identifier Prefix { get; set; }

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